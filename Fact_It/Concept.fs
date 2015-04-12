namespace Concept

module FactExp =    
    type Fact<'key,'a> = 
        | Journaled of ('key * 'a) // Represents an event executed and logged
        | MeantToBe of 'a // Represent the conclusion of the emminent need to execute an action and journal the fact
        | NotMeantToBe // Represent the conclusion that there is not action to be executed

    type FactBuilder<'key, 'a> 
            ( gather : unit -> Option<('key * 'a)> , // This lambda retrieves the fact if it has already been computer 
              letItBe : 'a -> 'a , // This lambda executes all side effects related to the Fact (i.e. writting to disk, DB, queues, Web API calls, FTP, etc...)
              journal : 'a -> ('key * 'a) ) = // The last lambda journals the Fact.

        // Checks to see if the Fact was already computed and Journled.  If so, the stored informatio is provided. 
        // If not the computation is executed
        member this.Delay f =
            match gather() with
            | Some  b -> Journaled b
            | None -> f()

        // Composes different Facts
        member this.Bind ( m ,  f ) =
            match m with 
            | Journaled (key , a) -> f a
            | MeantToBe a -> f a
            | _ -> NotMeantToBe  

        // Executes side effects and journals the fact 
        member this.Return a = letItBe a |> journal |> Journaled 

        // returns a Fac<'key,'a>, if Meant-To-Be executes side effects and journals the fact
        member this.ReturnFrom m =
            match  m with 
            | Journaled _ -> m
            | MeantToBe a -> this.Return a
            | _ -> NotMeantToBe
