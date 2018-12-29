var Rx = require('rxjs/Rx');
function Emit(observer){
    var x = 0;
    x=5;
    observer.next(x);
    x=6;
    observer.next(x);
    x=11;
    observer.next(x);
}
var myObservable = Rx.Observable.create(observer => Emit(observer));
  myObservable.subscribe({
    next: x => console.log('got value ' + x)
  });