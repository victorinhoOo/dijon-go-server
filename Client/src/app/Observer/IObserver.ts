import { Observable } from "./Observable";

export interface IObserver{
    update(object: Observable):void;
}