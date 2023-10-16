This is an assignment for the Cloud Databases course. This assignment entails making a mortgage application where customers can sign-up for a mortgage. 

How the application works:
1. You have to sign-up for a mortgage as one or two customers. No more, no less. If you have two customers, the anual income will be shared between the two people. This is done through the POST endpoint in the mortgages.
2. As a customer, you can only have one mortgage application active at a time.
3. When the mortgage application is sent, it will be processed along with all of the other application that were sent that on at 11:59:59PM (just before midnight)
4. In the morning, at around 6-7AM an email will be sent to all the customers who applied for a mortgage where they can view their mortgages (through a localhost endpoint)

Additional points:
1. This application contains a data seeder which will populate the Cosmos DB with 4 random users and 3 mortgage applications.
2. In order to view this, you will need to download the Cosmos DB Emulator as this is all done in localhost: https://learn.microsoft.com/en-us/azure/cosmos-db/how-to-develop-emulator?tabs=windows%2Ccsharp&pivots=api-nosql. You do not have to configure anything else as all localhosts have the same information and all of this is already configured within the application.
3. As mentioned before, the relationship between the customer and the mortgage is 1-many. Although the logic is made so that you can have a max of 2 customers per mortgage. I mean, it would be weird if you had more than one partner, no?
4. The send grid client should be able to send an email to any domain, feel free to use any account (a fake one would be more desirable)
   
Reasoning behind the database and architecture thereof:
1. CosmosDB is able to handle faster load of data over a short period of time.
2. CosmosDB is a NoSQL database, meaning it does not require a schema to make. The development process is really quick, and if any database or tables need to be dropped it's instant, whereas in SQL you have to consider foreign key constrains.
3. CosmosDB provides a way to define a JSON-like schema where you can embed or reference various objects.
4. In terms of the database structure, the "embedding" method was chosen. After comparing various websites, I noticed that they do not require the user(s) to sign-in anywhere. Instead, you specify who you are, how many people there are and what your anual income is. From that you can get the mortgage. Hence why I embedded the customers inside the mortgage and made it just a singular table in the database. This makes life much easier.
5. Pretty much all services are loosely coupled which ensures that they can be individually tested through mocks or any other kind of unit test.
6. The azure functions are made in different project because they are supposed to serve as a stand-alone function/project. Inside there, the required services are injected and registered accordingly.
