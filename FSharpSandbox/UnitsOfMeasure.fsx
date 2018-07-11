[<Measure>] type km
[<Measure>] type h

let length = 9.0<km>

let square (l:float<km>) = l * l

let distanceInTwoHours(speed:float<km/h>) =
    speed * 2.0<h>

let speedForTwoHours(distance:float<km>) = 
    distance / 2.0<h>

    
[<Measure>] type percent
let coef = 33.0<percent>

let wrong = 50.0<km> * coef

let good = 50.0<km> * coef / 100.0<percent>