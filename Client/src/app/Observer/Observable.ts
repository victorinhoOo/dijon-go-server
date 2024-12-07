import { IObserver } from "./IObserver"

export abstract class Observable{
    private observers: Array<IObserver>

    public constructor(){
        this.observers = new Array<IObserver>();
    }

    public register(observer:IObserver){
        this.observers.push(observer);
        console.log(this.observers.length);
    }

    protected notifyChange(object:Observable){  
        this.observers.forEach(obs=>{
            obs.update(object);
        })
    }
}