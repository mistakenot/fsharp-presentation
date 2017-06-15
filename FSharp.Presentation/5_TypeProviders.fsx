(* 5. Type Providers
    - Type providers give you strongly typed access
        to external datasources.
    - Sql, Json, Web Service, OData etc...
*)
// Swagger type provider
#r "../packages/SwaggerProvider.0.6.1/lib/net45/SwaggerProvider.dll"
#r "../packages/SwaggerProvider.0.6.1/lib/net45/SwaggerProvider.Runtime.dll"
open SwaggerProvider

type Petstore = SwaggerProvider<"http://petstore.swagger.io/v2/swagger.json">

let store = Petstore()

let user = store.GetUserByName("AuthorisedTestUser")

let newPet = new Petstore.Pet()
newPet.Name <- "Fluffy"
store.AddPet(newPet)

let order = new Petstore.Order()
order.PetId <- newPet.Id
store.PlaceOrder(order)




// OData type provider
#r "FSharp.Data.TypeProviders.dll"
#r "System.Data.Services.Client.dll"
open Microsoft.FSharp.Data.TypeProviders
open System.Linq

type Northwind = ODataService<"http://services.odata.org/Northwind/Northwind.svc">
let db = Northwind.GetDataContext()


// Using C# linq
let getNamesOfManchesterCustomers = 
    db.Customers.Take(10).Select(fun c -> c.ContactName)


// Using F# query expression
let expressionQuery = 
    query {
        for customer in db.Customers do
        take 10
        select customer
    }

expressionQuery |> Seq.iter (fun customer -> printfn "%s" customer.ContactName)