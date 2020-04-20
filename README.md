# Course Management System

<p align="center"><img src="https://raw.githubusercontent.com/mhdb96/CMS/master/CMSUI/Images/Logo.png" width="200"></p>

## Overview:

A system for managing, storing and reporting students’ exam
results in a giving faculty with the ability to:

- Manage the courses, terms, teachers and assignments by the
  admin through the UI.
- Evaluate the uploaded tests’ results and export all the data to Excel file.

---

## How we developed the system:

We started by analyzing the given project file and tried to reduce the big project to a much small problems to make solving them more easy, then we took every problem and started to studying it and finding the optimal algorithm to solving it (like how we crated the Create Exam window to take firstly the Answer Keys and based on them creating the Exam Groups and the Questions Count for each group which will help us when evaluation the students answers).

After finishing the pervious step, we created the first flow chart diagram for the full project with the basic procedures foreach section of the program, and with that part out of the way we had a pretty good imagination of what the project will be like and based on that we started the design process.

As a first step we started with designing the database with its tables and the relations between them, although in the project requirement there was no mention for the need to store the student data or their answers in the database, we decided to store them for three reasons:

1. Limit the student creation process to the admins to reduce the probability of wrong data entry by the teachers.
1. Reduce and auto solve the mistakes made by students on the optic paper regarding their personal data like first and last name which will make the exam evaluation process easier for the teacher
1. By storing all the students answers in the database we allow the results to be edited without the need to reenter all the data and evaluation it once more and in case of a lost excel file, it can be recreated with a click of a button.

### Our exam creation process consists of:

1. Selecting an assignment
1. Creating an exam with a type and date
1. Based on the answer keys creating exam groups
1. Foreach question in the answer keys creating a question and assigning to it the wanted outcomes
1. Foreach question and student in a given exam group creating a result with the student’s answer

After finishing the design of the database, we started to design the program UI by sketching the wanted windows and panel with the basic functionality of each one.
With the ending of the early designing of the system we started the research phase for the project which consisted of these parts:

1. The Platform: according to our past skills and the given period of the project we decided to use the .Net platform on windows with C# language to develop the project with more advanced programing techniques and design patterns.
1. The UI Type: due to the lack of UI scaling feature in windows form and the fact that it is using ancient technology for displaying content we decided to use WPF with MahApp.Metro framework to achieve a good looking, responsive and scalable UI with minimum effort in addition to the ability to design the UI separated from the code behind which gave us flexibility in making updates and improvement to the UI without the need to change the back-end side of the forms.
1. Database: as mentioned above we decided to go with C# as the programming language for the project we didn’t find a better solution than MS SQL Serve to handle the database for the following reasons:
   - Being developed by Microsoft assures the compatibility and optimization for Windows
   - Better compatibility and speeds when working with C#

---

## Used Technologies:

1. Visual Studio 2017 for C# development.
1. Ms SQL Server for database.
1. Dapper for connecting the C# (back-end) to the database with its API.
1. MahApps.Metro as a WPF library for UI design.
1. MahApps.Metro.IconPack as an icon library for UI design.
1. Squirrel.Windows for deploying the app as an oneClick executable.
1. Office.Core for writing data to Excel.
1. XAML language to create the UI for WPF app.
1. GitHub for version control and hosting the app releases online.

---

## Programing Techniques:

- Business Logic:
  1. We opted for the use of a class library (CMSLibrary) to separate the bossiness logic from the UI completely and to give us the flexibility to change the UI in the future without the need to change anything in the library.
  1. In CMSLibrary we have an IDataConnection interface specifying the required functions for a DataConnection class whether it’s a SQL or MySQL or any different Database which will make it easier to migrate to a new database if needed with writing more than a one class
  1. We used Enums for our in-program options which made the development process easier and error free.
  1. We used Data Annotations for validating the user input in the app in real-time with superior decrees in code-repetition according to classic if-else validation.
  1. The using of models to represent every database table in the back-end with some special property made our job much easier without the need to make
     unnecessary queries to the database furthermore made the data exchanging between different UI element easier and more efficient.
  1. Making all the database’s info dynamic and not hard coded which make connecting to a new database easier.
- The User Interfacez

  1. Using of converters to change values in the UI in real-time to make it more dynamic.
  1. Using of requesters (Interface) to loose couple windows to each other so any window can call any other window as long as it implements its requester interface (Data Exchange).
  1. Using of dependency property to send data to children user controls.
  1. Using of User Control for all the dashboard and small elements which allowed us to use these controls in multiples places without the need to rewrite them and with a one place for editing which had a huge effect on our work-flow while designing the UI (UserControls Folder).
  1. Using MahApps theme manager to change the app mode (Dark – Light) and accent colors on the fly in runtime.
  1. Using flyouts to display the Help section in a neat way.

- Database:
  1. We used stored procedures for the added security against SQL injections attacks and for the gained speed thanks to caching.
  1. We used Microsoft SQL Server Management Studio to managing our SQL server.

#### Technologies

- MS SQL Server.
- Dapper.
- MahApps.Metro for UI design.
- Office.Core.
- Squirrel for deploying the app.
- GitHub for version control.

## Developers' Info

- **Muhammed Bedavi** ~ [Linkedin🔗](https://www.linkedin.com/in/mhdb96/) - [GitHub🔗](https://github.com/mhdb96)
- **Onur KANTAR** ~ [Linkedin🔗](https://www.linkedin.com/in/onur-kantar-580ab1ab/) - [GitHub🔗](https://github.com/simiyen)
