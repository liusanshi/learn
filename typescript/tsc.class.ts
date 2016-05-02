/**
 * Subject
 */
class Subject {
    
    public static StatVar : number = 1; 
        
    public get myVal() : string {
        return this.Name;
    }
    
    public set myVal(v : string) {
        if(Subject.StatVar >= 0){
            this.Name = v;   
        } else {
            alert('asdasd');
        }
    }
    
    
    
    constructor(public ID : number, public Name: string) {
        // this.Name = name;
        // this.ID = id;
    }
    
    public toString(){
        return this.ID + ':' + this.Name;
    }
}

interface InterfaceObj{
    x : number;
    y : number;
}

interface InterfaceFunc{
    (x : number, y :string) : void;
}

var obj : InterfaceObj = {x : 1, y : 1};
var func : InterfaceFunc = (x,y) => console.log(x.toString() + y);

var sub = new Subject(1, 'asdasd');
sub.myVal = "2";

var arr:(string|number)[] = [1,2,3,4,5];
arr = ['1','2',3,4,5];

// var SubjectMark : typeof Subject = Subject;
// var sub2 = new SubjectMark();

// sub2.myVal = 2;


// printDelayed 返回值是一个 'Promise<void>'
async function printDelayed(elements: string[]) {
    for (const element of elements) {
        await delay(200);
        console.log(element);
    }
}

async function delay(milliseconds: number) {
    return new Promise<void>(resolve => {
        setTimeout(resolve, milliseconds);
    });
}

printDelayed(["Hello", "beautiful", "asynchronous", "world"]).then(() => {
    console.log();
    console.log("打印每一个内容!");
});

// console.log(sub2.toString());
console.log(sub.toString());
console.log(Subject.StatVar);