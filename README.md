This is my first experimantation with Computational Expressions in FSharp.

The main goal of this excercise is to define a clear distinction between the pure logic
of a computation and any infrastuctural concerns/side effects.

Important concepts:

Journaled Fact:  indesputable truth, immutable record of an event that happened
Meant-To-Be a Fact:  Derived conclusion of the imminent need to execute an action and journal the event
Not-Menat-To-Be a Fact: Derived conclusion of no action needed.

Deriving the conclusion involves pure computation

Executing the Action (letting the Fact "be") implies Side Effects

Journaling the executed action (the Fact) implies a Side Effect and it should only be executed after the Action has completed

Before computing the conclusion, the computational expresion will try to "Gather" the Fact to check if it has been computed and executed before.  If so, no computation or action will be executed, instead the stored values will be provided.

I developer the code in Xamarin Studio.  To run type at the Fsharp interactive console: 

&#load "[ROOT OF PROJECT]\Concept.fs";;
&#load "[ROOT OF PROJECT]\Calculation.fs";;
Calculations.ToyProblem.finalFact ();;

i.e.

&#load "C:\Users\Luis\Documents\Projects\Fact_It\Fact_It\Concept.fs";;
&#load "C:\Users\Luis\Documents\Projects\Fact_It\Fact_It\Calculations.fs";;
Calculations.ToyProblem.finalFact ();;

Result:

Gathering key 2
Gathering key 0
Side Effect for value 1
Journaling key 0 value 1
Gathering key 1
Gathering key 0
Side Effect for value 2
Journaling key 1 value 2
Side Effect for value 6
Journaling key 2 value 6
val it : Concept.FactExp.Fact<int,int> = Journaled (2, 6)

Subsequent calls will result in:

Gathering key 2
val it : Concept.FactExp.Fact<int,int> = Journaled (2, 6)

Please see code comments for more information