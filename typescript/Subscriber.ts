namespace Rx.Observable {
    /**
     * 空函数的接口
     */
    export interface Noop {
        (val: any): void
    }
    /**
     * 抛出异常的接口
     */
    export interface throwError {
        (e: any): void
    }

    /**
     * Subscriber
     */
    export class Subscriber1 {
        private isStopped: boolean = false;
        constructor(private destination: { next?: Noop, error?: throwError, complete?: Noop }) {
        }

        /**
         * 正常时触发
         */
        next(val: any): void {
            if (!this.isStopped) {
                this.destination.next(val);
            }
        }

        /**
         * 错误时触发
         */
        error(err: any): void {
            if (!this.isStopped) {
                this.isStopped = true;
                this.destination.error(err);
            }
        }

        /**
         * 结束
         */
        complete(val: any): void {
            if (!this.isStopped) {
                this.isStopped = true;
                this.destination.complete(val);
            }
        }

        /**
         * 创建一个订阅
         */
        static create(next: Noop, error: throwError, complete: Noop): Subscriber1 {
            return new Subscriber1({ "next": next, "error": error, "complete": complete });
        }
    }
}