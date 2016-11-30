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
<?php
/**
 * 逻辑单元的接口
 * Interface ILogicUnit
 */
interface ILogicUnit{
    //处理
    public function process($context, $data = null);
}

/**
 * 逻辑流程控制基类
 * Class LogicUnitBase
 */
abstract class LogicUnitBase {
    /**
     * 下一个逻辑处理单元
     * @var LogicUnitBase
     */
    protected $nLogicUnit = null;

    /**
     * 逻辑实体
     * @var ILogicUnit
     */
    protected $LogicUnit = null;

    /**
     * 接入下一个流程
     * @param LogicUnitBase $lp
     * @return mixed
     */
    public function pipe($lp){
        if($lp instanceof LogicUnitBase){
            $this->nLogicUnit = $lp;
            return $lp;
        }
        throw new Exception('args class error');
    }

    /**
     * 执行下个流程
     * @param LogicUnitContext $context
     * @param null $data
     * @return mixed
     */
    public function next($context, $data = null){
        if(!$context -> isEnd()) { //如果结束了就不会继续传播
            $context->CurLogicUnit = $this->nLogicUnit;
            $this->nLogicUnit->exec($context, $data);
        }
    }

    /**
     * 执行当前逻辑 可以重入
     * @param $context
     * @param null $data
     * @return mixed
     */
    abstract public function exec($context, $data = null);
}

/**
 * 简单逻辑处理单元
 * Class LogicalUnit
 */
class SimpleLogicUnit extends LogicUnitBase {
    /**
     * SimpleLogicUnit constructor.
     * @param ILogicUnit $lu
     */
    public function __construct($lu = null) {
        if($lu){
            if($lu instanceof ILogicUnit){
                $this->LogicUnit = $lu;
            } else {
                throw new Exception('args class error');
            }
        }
    }
    //是否执行
    private $_IsExecute = false;

    /**
     * 执行当前逻辑 可以重入
     * @param $context
     * @param null $data
     * @return mixed
     */
    public function exec($context, $data = null){
        if(!$this->_IsExecute){
            $this->_IsExecute = true;
            if($this->LogicUnit){
                $this->LogicUnit->process($context, $data);
                return;
            }
        }
        $this->next($context, $data);
    }
}
/**
 * 逻辑处理单元的上下文基类
 * Class LogicUnitContext
 */
class LogicUnitContext{

    public function __construct($logicunit = null) {
        if($logicunit){
            if($logicunit instanceof LogicUnitBase){
                $this->EndLogicUnit = $logicunit;
            } elseif($logicunit instanceof ILogicUnit){
                $this->EndLogicUnit = new SimpleLogicUnit($logicunit);
            }
        }

    }

    public function __destruct() {
        $this->destroy();
    }

    //是否结束
    private $IsEnd = false;
    /**
     * 最后结束时的逻辑处理
     * @var null|SimpleLogicUnit
     */
    private $EndLogicUnit = null;
    /**
     * 当前执行的逻辑
     * @var LogicUnitBase
     */
    public $CurLogicUnit = null;
    //全局环境变量
    public $Env_Var = array();

    /**
     * 通知执行完毕
     * @param $data
     * @return mixed
     */
    function notifyEnd($data = null){
        $this->next(true, $data);
    }

    public function next($isend = false, $data = null){
        if($isend){
            $this->IsEnd = true;
            if($this->EndLogicUnit){
                $this->EndLogicUnit->exec($this, $data); //通知结束了
            }
        } elseif($this->CurLogicUnit && $this->CurLogicUnit instanceof LogicUnitBase) {
            $this->CurLogicUnit->exec($this, $data);
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

    /**
     * 销毁
     */
    public function destroy(){
        if($this->EndLogicUnit){
            unset($this->EndLogicUnit);
        }
        if($this->CurLogicUnit){
            unset($this->CurLogicUnit);
        }
        if($this->Env_Var){
            unset($this->Env_Var);
        }
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
 * 控制流程的逻辑单元处理类
 * Class LogicUnitProcessLogicUnit
 */
class LogicUnitProcessLogicUnit implements ILogicUnit{

    /**
     * LogicUnitProcessLogicUnit constructor.
     * @param LogicUnitBase $prev
     * @param LogicUnitBase $after
     */
    public function __construct($prev , $after) {
        $this->Prev = $prev;
        $prev->pipe($after);
    }

    /**
     * @var LogicUnitBase
     */
    private $Prev = null;

    public function process($context, $data = null) {
        $context->CurLogicUnit = $this->Prev;
        $this->Prev->exec($context, $data);
    }
}

/**
 * 并行逻辑处理类（不是物理上的并行，是逻辑上的并行）
 * Class ParallelLogicUnit
 */
class ParallelLogicUnit extends LogicUnitBase {

    public function __construct($mergecond = null) {
        if(!$mergecond){ //默认的合并条件
            $mergecond = new DefaultMergeCondition();
        }
        $this->MergeCondition = $mergecond;
        $this->LogicUnit = array();
    }

    public function __destruct() {
        $this->destroy();
    }

    /**
     * 合并逻辑的条件
     * @var IMergeCondition
     */
    private $MergeCondition = null;
    //当前执行的序号
    private $_index = 0;
    //结果集合
    private $_ResultCollect = array();

    /**
     * 追加逻辑处理单元
     * @param ILogicUnit $lu
     */
    public function push($lu){
        if($lu instanceof ILogicUnit){
            $this->LogicUnit[] = $lu;
            return $this;
        } elseif($lu instanceof LogicUnitBase){
            $this->LogicUnit[] = new LogicUnitProcessLogicUnit($lu, $this);
            return $this;
        } else {
            throw new Exception('args type error');
        }
    }

    /**
     * 执行所有逻辑 可以重入
     * @param LogicUnitContext $context
     * @param array $data
     */
    public function exec($context, $data = null) {
        $count = count($this->LogicUnit);
        if($this->MergeCondition->canMerge($context, $count, $this->_ResultCollect)){//允许进入下一个逻辑
            $this->next($context, $this->_ResultCollect);
        } elseif(isset($this->LogicUnit[$this->_index])){
            $this->_ResultCollect[] = $this->LogicUnit[$this->_index++]->process($context, $data);
        } else {
            $this->next($context, $this->_ResultCollect);
        }
    }

    /**
     * 销毁
     */
    public function destroy(){
        if($this->MergeCondition){
            unset($this->MergeCondition);
        }
        if($this->LogicUnit){
            unset($this->LogicUnit);
        }
    }
}

/**
 * 串行的逻辑处理类
 * Class SerialLogicUnit
 */
class SerialLogicUnit extends LogicUnitBase{

    public function __construct() {
        $this->LogicUnit = array();
    }

    public function __destruct() {
        $this->destroy();
    }

    //当前执行的序号
    private $_index = 0;
    //结果集合
    private $_ResultCollect = array();

    /**
     * 接上
     * @param $lu
     */
    public function push($lu){
        if($lu instanceof ILogicUnit){
            $this->LogicUnit[] = $lu;
            return $this;
        } elseif($lu instanceof LogicUnitBase){
            $this->LogicUnit[] = new LogicUnitProcessLogicUnit($lu, $this);
            return $this;
        } else {
            throw new Exception('args type error');
        }
    }

    /**
     * 执行当前逻辑 可以重入
     * @param LogicUnitContext $context
     * @param null $data
     */
    public function exec($context, $data = null) {
        if(isset($this->LogicUnit[$this->_index])){
            $this->_ResultCollect[] = $this->LogicUnit[$this->_index++]->process($context, $data);
        } else {
            $this->next($context, $this->_ResultCollect);
        }
    }
    /**
     * 销毁
     */
    public function destroy(){
        if($this->LogicUnit){
            unset($this->LogicUnit);
        }
        if($this->_ResultCollect){
            unset($this->_ResultCollect);
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
        $this->Frist = new SimpleLogicUnit();
        $this->Current = $this->Frist;
    }

    /**
     * 接上
     * @param $lu
     */
    public function pipe($lu){
        $lp = null;
        if($lu instanceof ILogicUnit){
            $lp = new SimpleLogicUnit($lu);
        } elseif($lu instanceof LogicUnitBase){
            $lp = $lu;
        }
        if($lp){
            $this->Current->pipe($lp);
            $this->Current = $lp;
            return $this;
        } else {
            throw new Exception('args type error');
        }
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

class LUA extends ILogicUnit{
    public function process($context, $data = null) {

        echo "处理逻辑： A\n";

        $context->next($context); //继续下一个逻辑
//        $context->notifyEnd();   //结束整个逻辑执行
    }
}

class LUB extends ILogicUnit{
    public function process($context, $data = null) {

        echo "处理逻辑： B\n";
//        $context->notifyEnd();   //结束整个逻辑执行
        $context->next($context);
    }
}

class LUEnd extends ILogicUnit{
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
