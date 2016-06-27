<?php
/**
 * Created by PhpStorm.
 * User: payneliu
 * Date: 2016/6/27
 * Time: 15:48
 */

/*
 * Code平台-微码 全新支持 php 语言在线运行
 */

/**
 * 逻辑处理单元
 * Class LogicalUnit
 */
abstract class LogicUnitBase{

    //处理单元列表
    private $LogicUnits = array();
    //列表的数量
    private $Count = 0;

    /**
     * 对接下一个LogicUnit
     * @param LogicUnitBase $lu
     */
    public function pipe($lu){
        $this->LogicUnits[] = $lu;
        $this->Count++;
        return $lu;
    }

    /**
     * 完成执行下一个
     * @param $context
     * @param $data
     */
    public function next($context, $data = null){
        if($this->Count === 1){ //大部分跑这里，进行性能优化
            $this->LogicUnits[0]->process($context, $data);
        } elseif($this->Count > 1){
            foreach($this->LogicUnits as $lu){
                if(!$context -> isEnd()){ //如果结束了就不会继续传播
                    $lu->process($context, $data);
                }
            }
        }
    }

    //处理
    public abstract function process($context, $data = null);
}

/**
 * 逻辑处理单元的上下文基类
 * Class LogicUnitContext
 */
class LogicUnitContext{

    public function __construct($logicunit) {
        $this->EndLogicUnit = $logicunit;
    }

    //是否结束
    private $IsEnd = false;
    //最后结束时的逻辑处理
    private $EndLogicUnit = null;
    //全局环境变量
    public $Env_Var = array();

    /**
     * 通知执行完毕
     * @param $data
     * @return mixed
     */
    function notifyEnd($data = null){
        $this->IsEnd = true;
        if($this->EndLogicUnit){
            $this->EndLogicUnit->process($this, $data); //通知结束了
        }
    }

    /**
     * 是否已经执行完毕
     * @return bool
     */
    public function isEnd(){
        return $this->IsEnd;
    }

    /**
     * 克隆一份上下文关系
     * @param $logicunit
     * @return LogicUnitContext
     */
    public function copy($logicunit){
        $luc = new LogicUnitContext($logicunit);
        $luc->Env_Var = $this->Env_Var;
        return $luc;
    }
}

/**
 * 默认逻辑处理单元
 * Class DefaultLogicUnit
 */
class DefaultLogicUnit extends LogicUnitBase{
    public function process($context, $data = null) {
        $this->next($context, $data);
    }
}

/**
 * 逻辑收拢条件
 * Interface IMergeCondition
 */
interface IMergeCondition{
    /**
     * 是否可以合并逻辑
     * @param $context
     * @param $logicnum
     * @param $resultarr
     * @return bool
     */
    function canMerge($context, $logicnum, $resultarr);
}

/**
 * 默认的合并逻辑的条件，即所有逻辑都处理完成才进入下一个逻辑
 * Class DefaultMergeCondition
 */
class DefaultMergeCondition implements IMergeCondition{
    /**
     * 是否可以合并逻辑 当所有逻辑都执行完了之后才会进入下一个逻辑
     * @param $context
     * @param $logicnum
     * @param $resultarr
     * @return bool
     */
    public function canMerge($context, $logicnum, $resultarr){
        return $logicnum === count($resultarr);
    }
}

/**
 * 收拢聚集类
 * Class MergeLogicUnit
 */
class MergeLogicUnit extends LogicUnitBase{

    public function __construct($parentlu, $mergecond) {
        $this->ParentLU = $parentlu;
        $this->MergeCondition = $mergecond;
    }

    public function __destruct() {
        if($this->ParentLU){
            unset($this->ParentLU);
        }
    }

    private $ParentLU = null;
    /**
     * 记录有多少个单元需要聚拢
     * @var int
     */
    private $Count = 0;
    /**
     * 合并逻辑的条件
     * @var IMergeCondition
     */
    private $MergeCondition = null;

    /**
     * 聚拢的执行结果
     * @var array
     */
    private $ResultCollect = array();
    /**
     * 是否结束
     * @var bool
     */
    private $IsEnd = false;

    /**
     * 添加需要需要聚拢的单元数量
     */
    public function inc(){
        $this->Count++;
    }

    /**
     * 是否结束
     * @return bool
     */
    public function isEnd(){
        return $this->IsEnd;
    }

    public function process($context, $data = null) {
        $this->ResultCollect[] = $data;
        if(!$this->IsEnd && $this->MergeCondition->canMerge($context, $this->Count, $this->ResultCollect)){//允许进入下一个逻辑
            $this->IsEnd = true;
            $this->ParentLU->next($context, $this->ResultCollect);
        }
    }
}

/**
 * 并行逻辑处理类（不是物理上的并行，是逻辑上的并行）
 * Class ParallelLogicUnit
 */
class ParallelLogicUnit extends LogicUnitBase{

    public function __construct($mergecond = null) {
        if(!$mergecond){ //默认的合并条件
            $mergecond = new DefaultMergeCondition();
        }
        $this->MergeLogicUnit = new MergeLogicUnit($this, $mergecond);
    }

    public function __destruct() {
        $this->destroy();
    }

    //处理单元列表
    private $innerLogicUnits = array();
    /**
     * 收拢的逻辑处理单元
     * @var MergeLogicUnit
     */
    private $MergeLogicUnit = null;

    /**
     * 追加逻辑处理单元
     * @param $lu
     */
    public function push($lu){
        $this->MergeLogicUnit->inc();
        $lu->pipe($this->MergeLogicUnit); //所有组合单元都接到内部收敛逻辑单元
        $this->innerLogicUnits[] = $lu;
        return $this;
    }

    /**
     * 执行所有逻辑
     * @param $context
     * @param array $data
     */
    public function process($context, $data = null) {
        foreach($this->innerLogicUnits as $lu){
            if(!$this->MergeLogicUnit -> isEnd()){ //如果结束了就不会继续传播
                $lu->process($context, $data);
            }
        }
    }

    /**
     * 销毁
     */
    public function destroy(){
        if($this->MergeLogicUnit){
            unset($this->MergeLogicUnit);
        }
        if($this->innerLogicUnits){
            unset($this->innerLogicUnits);
        }
    }
}

/**
 * 串行的逻辑处理类
 * Class SerialLogicUnit
 */
class SerialLogicUnit extends LogicUnitBase{

    public function __construct() {
        $this->MergeLogicUnit = new MergeLogicUnit($this, new DefaultMergeCondition());
    }

    public function __destruct() {
        $this->destroy();
    }

    /**
     * 第1个处理单元
     * @var LogicUnitBase
     */
    private $Frist = null;
    /**
     * 当前的处理单元
     * @var LogicUnitBase
     */
    private $Current = null;
    /**
     * 转接单元
     * @var MergeLogicUnit|null
     */
    private $MergeLogicUnit = null;

    /**
     * 是否第一次执行
     * @var bool
     */
    private $IsFristRun = true;

    /**
     * 接上
     * @param $lu
     */
    public function push($lu){
        if(!$this->Frist){
            $this->Frist = $lu;
        } else {
            $this->Current->pipe($lu);
        }
        $this->Current = $lu;
        return $this;
    }

    /**
     * 执行逻辑
     * @param $context
     * @param array $data
     */
    public function process($context, $data = null) {
        if($this->Frist){
            if($context instanceof LogicUnitContext){
                if($this->IsFristRun){ //第一次执行 添加结束的处理单元
                    $this->IsFristRun = false;
                    $this->MergeLogicUnit->inc();
                    $this->Current->pipe($this->MergeLogicUnit);
                }

                $this->Frist->process($context, $data);
            } else {
                throw new Exception('context type error');
            }
        }
    }

    /**
     * 销毁
     */
    public function destroy(){
        if($this->MergeLogicUnit){
            unset($this->MergeLogicUnit);
        }
    }
}

/**
 * 逻辑单元的容器
 * Class LogicContainer
 */
class LogicContainer{
    /**
     * 第1个处理单元
     * @var LogicUnitBase
     */
    private $Frist = null;

    /**
     * 当前的处理单元
     * @var LogicUnitBase
     */
    private $Current = null;
    public function __construct() {
        $this->Frist = new DefaultLogicUnit();
        $this->Current = $this->Frist;
    }

    /**
     * 接上
     * @param $lu
     */
    public function pipe($lu){
        $this->Current->pipe($lu);
        $this->Current = $lu;
        return $this;
    }

    public function run($context, $data = null){
        if($context instanceof LogicUnitContext){
            $this->Frist->next($context, $data);
        } else {
            throw new Exception('context type error');
        }
    }
}

//使用方法：===================================================

class LUA extends LogicUnitBase{
    public function process($context, $data = null) {

        echo "处理逻辑： A\n";

        $this->next($context); //继续下一个逻辑
//        $context->notifyEnd();   //结束整个逻辑执行
    }
}

class LUB extends LogicUnitBase{
    public function process($context, $data = null) {

        echo "处理逻辑： B\n";
//        $context->notifyEnd();   //结束整个逻辑执行
        $this->next($context);
    }
}

class LUEnd extends LogicUnitBase{
    public function process($context, $data = null) {

        echo "处理结束\n";
    }
}

class MyMergeCondition implements IMergeCondition{
    public function canMerge($context, $logicnum, $resultarr){
        return count($resultarr) >= 1; //可以控制并行执行的数量
    }
}

//LogicUnitContext
$context = new LogicUnitContext(new LUEnd());
$container = new LogicContainer();

$serial = new SerialLogicUnit(); //串行的逻辑
$muti = new ParallelLogicUnit(new MyMergeCondition()); //并行的逻辑
$muti->push($serial->push(new LUA())->push(new LUB())->push(new LUB()))
    ->push(new LUA())
    ->push(new LUA())
    ->push(new LUA());

$container->pipe(new LUB())
    ->pipe($muti)
    ->pipe(new LUB())
    ->pipe(new LUEnd())
    ->run($context);
