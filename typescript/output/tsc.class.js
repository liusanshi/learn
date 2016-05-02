var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator.throw(value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments)).next());
    });
};
class Subject {
    constructor(ID, Name) {
        this.ID = ID;
        this.Name = Name;
    }
    get myVal() {
        return this.Name;
    }
    set myVal(v) {
        if (Subject.StatVar >= 0) {
            this.Name = v;
        }
        else {
            alert('asdasd');
        }
    }
    toString() {
        return this.ID + ':' + this.Name;
    }
}
Subject.StatVar = 1;
var obj = { x: 1, y: 1 };
var func = (x, y) => console.log(x.toString() + y);
var sub = new Subject(1, 'asdasd');
sub.myVal = "2";
var arr = [1, 2, 3, 4, 5];
arr = ['1', '2', 3, 4, 5];
function printDelayed(elements) {
    return __awaiter(this, void 0, void 0, function* () {
        for (const element of elements) {
            yield delay(200);
            console.log(element);
        }
    });
}
function delay(milliseconds) {
    return __awaiter(this, void 0, void 0, function* () {
        return new Promise(resolve => {
            setTimeout(resolve, milliseconds);
        });
    });
}
printDelayed(["Hello", "beautiful", "asynchronous", "world"]).then(() => {
    console.log();
    console.log("打印每一个内容!");
});
console.log(sub.toString());
console.log(Subject.StatVar);
//# sourceMappingURL=tsc.class.js.map