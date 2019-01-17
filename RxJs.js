var Rx = require('rxjs/Rx');

function Emit(observer){
    var x = 0;
    x=5;
    observer.next(x);
    x=16;
    observer.next(x);
    x=11;
    observer.next(x);
    observer.error("bad");
    observer.complete();

}
var myObservable = new  Rx.Observable(Emit);
  myObservable.
    filter(x=>x>10)
    .subscribe({
    next: x => console.log('got value ' + x),
    complete: () => console.log('Completed'),
    error:e=>console.log(e)
  });
