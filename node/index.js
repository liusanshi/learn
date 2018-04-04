let koa = require('koa');
let _ = require('koa-route');
let app = new koa();

// app.use(ctx => {
//     ctx.body = 'jahahhaha';
// });

const db = {
    tobi: { name: 'tobi', species: 'ferret' },
    loki: { name: 'loki', species: 'ferret' },
    jane: { name: 'jane', species: 'ferret' }
};

const pets = {
    list(){
        let names = Object.keys(db);
        this.body = 'pets: ' + names.join(',');
    },
    show(ctx, name){
        let pet = db[name];
        if(!pet){
            return this.throw('cannot find that pet', 404);
        }
        this.body = pet.name + ' ' + pet.species;
    }
};

app.use(_.get('/pets', pets.list));
app.use(_.get('/pets/:name', pets.show));

app.use(async (ctx, next) => {
    const begin = new Date();
    ctx.body = 'jahahhaha';
    await next();
    const time = new Date() - begin;
    console.log(`${ctx.method} ${ctx.url} - ${time}ms`);
});

app.listen(3000);