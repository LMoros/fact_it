namespace Calculations
 
module ToyProblem = 
    // Three facts are calculated.  
    // The third fact uses the first and the second facts to compute its result
    // The second fact uses the first fact to compuate its result
    // First fact computation is purposely slow.  Eventhough it is referenced twice, it is only computed once
    // Side effects are expressed separate from the busines logic
    // Side effects are only executed once 
    open Concept.FactExp

    // In memory repository.  This could be a relational DB, NoSQL DB, Disk, etc...
    let repository = new System.Collections.Generic.Dictionary<int,int>()

    // Side Effect.  This could be writting to Disk, to a DB, Sending message to a queue, Put or Post Restful calls, etc...
    let show a = 
        printfn "Side Effect for value %d" a 
        a  
              
    // Registers the fact in the repository
    let journal a = 
        let index = repository.Count 
        repository.Add(index, a)
        printfn "Journaling key %d value %d" index a 
        (index, a)

    // Generic function to gather a computed fact based on an index
    let gather i =
        printfn "Gathering key %d" i 
        if repository.ContainsKey i then
            Some ( i , repository.Item(i) )
        else
            None

    // specific gather function for first fact (index 0)
    let gather1 () = gather 0

    // specific gather function for second fact (index 1)
    let gather2 () = gather 1

    // specific gather function for third fact (index 2)
    let gather3 () = gather 2

    // Partially defining basic fact ( the fetch function is not defined so different gathering criterion can be apply for different facts)
    let myFact fetch = new FactBuilder<int , int> (fetch , show , journal ) 

    // First fact    
    let thisFact () = myFact gather1 {
         
        return  Seq.fold (fun acc x -> acc + x - x) 1 { 1..9999999 } // Slow running computation.  Should only run once.  
                                                                     // Subsequent references will use the journaled results 
    }

    // Second fact
    let thatFact () = myFact gather2 {
        let! this = thisFact ()
        if this  > 0 then 
            return this + 1  
        else
            return! NotMeantToBe      
    }

    // Final fact
    let finalFact () = myFact gather3 {
        let! this = thisFact () // This will trigger the slow running computation
        let! that = thatFact () // This will use the Journaled value of the first fact to calculate the second fact
        let final = (this + that) * that
        return final
    }