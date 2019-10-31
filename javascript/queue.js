const [Pending, Running, Resolved, Pause, Cancel] = [0, 1, 2, 3, 4]

const CancelError = new Error('cancel')

// 是否是取消的结果
export const isCancel = (result) => result === CancelError

/**
 * 创建一个队列的执行者
 *
 * Pending -> Running --------->   Resolved
 *               ^    |               ^
 *               |    | -> Cancel ____|
 *               |    | -> Pause
 *               |__________|
 *
 */
const buildQueueRunner = (queue = []) => {
  const list = [...queue]
  // status=> 0: pending; 1: running; 2: Resolved; 3: Pause; 4: cancel
  // index => 当前执行的顺序
  // result => 当前执行的结果
  // pauser => 用于暂停的promise对象
  // resumer => 用于恢复暂停的方法
  let [status, index, result, pauser, resumer] = [Pending, 0, null, null, null]
  return {
    /**
     * 执行队列
     * @param args
     * @returns {Promise<Error|*>}
     */
    async run (args) {
      if (status !== Pending) return result
      status = Running
      result = args
      for (let i = index, len = list.length; i < len; i++) {
        index = i
        const item = list[i]
        if (status === Cancel) { // Cancel
          return CancelError
        }
        if (status === Pause && pauser) { // Pause
          await pauser
        }
        result = await item(result)
      }
      status = Resolved
      return result
    },

    /**
     * 获取任务队列的执行结果
     * @returns {null}
     */
    getResult () {
      return result
    },

    /**
     * 获取当前的状态
     * @returns {number}
     */
    getStatus () {
      return status
    },

    /**
     * 暂停
     */
    pause () {
      status = Pause
      pauser = new Promise(resolve => {
        resumer = resolve
      })
    },

    /**
     * 恢复
     */
    resume () {
      if (resumer) {
        status = Running
        resumer()
      }
    },

    /**
     * 取消执行
     */
    cancel () {
      status = Cancel
    }
  }
}

/**
 * 任务队列
 *
 * @method run('hi').then(a => console.log(a)) : 执行任务，then为任务执行完成或者取消后执行；暂停恢复之后且执行完任务之后才跑
 * @method cancel() : 取消任务的执行
 * @method pause(): 暂停任务的执行
 * @method resume(): 恢复任务的执行
 * @method getResult(): 获取任务当前执行的结果
 * @method getStatus(): 获取任务当前执行的状态
 */
export default class Queue {
  constructor (list = []) {
    this.list = [...list]
    this.status = Pending // 0: pending; 1: running; 2: cancel
    this.runner = null
    this.index = -1 // 当前执行的顺序
  }

  /**
   * 添加任务
   * @param task Promise
   */
  addTask (task) {
    this.list.push(task)
  }

  /**
   * 执行任务
   */
  buildRunner () {
    return buildQueueRunner(this.list)
  }
}
