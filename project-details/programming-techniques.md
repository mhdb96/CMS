# Programming Techniques

### **Business Logic:**

1. We opted for the use of a class library \(CMSLibrary\) to separate the bossiness logic from the UI completely and to give us the flexibility to change the UI in the future without the need to change anything in the library.
2. In CMSLibrary we have an IDataConnection interface specifying the required functions for a DataConnection class whether it’s a SQL or MySQL or any different Database which will make it easier to migrate to a new database if needed with writing more than a one class
3. We used Enums for our in-program options which made the development process easier and error free.
4. We used Data Annotations for validating the user input in the app in real-time with superior decrees in code-repetition according to classic if-else validation.
5. The using of models to represent every database table in the back-end with some special property made our job much easier without the need to make unnecessary queries to the database furthermore made the data exchanging between different UI element easier and more efficient.
6. Making all the database’s info dynamic and not hard coded which make connecting to a new database easier.

### **The User Interfaces**

1. Using of converters to change values in the UI in real-time to make it more dynamic.
2. Using of requesters \(Interface\) to loose couple windows to each other so any window can call any other window as long as it implements its requester interface \(Data Exchange\).
3. Using of dependency property to send data to children user controls.
4. Using of User Control for all the dashboard and small elements which allowed us to use these controls in multiples places without the need to rewrite them and with a one place for editing which had a huge effect on our work-flow while designing the UI \(User-controls Folder\).
5. Using MahApps theme manager to change the app mode \(Dark – Light\) and accent colors on the fly in run-time.
6. Using fly-outs to display the Help section in a neat way.

### **Database:**

1. We used stored procedures for the added security against SQL injections attacks and for the gained speed thanks to caching.
2. We used Microsoft SQL Server Management Studio to managing our SQL server.

