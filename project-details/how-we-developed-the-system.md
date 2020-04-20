# How We Developed the System

We started by analyzing the given project file and tried to reduce the big project to a much small problems to make solving them more easy, then we took every problem and started to studying it and finding the optimal algorithm to solving it \(like how we crated the Create Exam window to take firstly the Answer Keys and based on them creating the Exam Groups and the Questions Count for each group which will help us when evaluation the students answers\).

After finishing the previous step, we created the first flow chart diagram for the full project with the basic procedures for each section of the program, and with that part out of the way we had a pretty good imagination of what the project will be like and based on that we started the design process.

As a first step we started with designing the database with its tables and the relations between them, although in the project requirement there was no mention for the need to store the student data or their answers in the database, we decided to store them for **three reasons:**

1. Limit the student creation process to the admins to reduce the probability of wrong data entry by the teachers.
2. Reduce and auto solve the mistakes made by students on the optic paper regarding their personal data like first and last name which will make the exam evaluation process easier for the teacher
3. By storing all the students answers in the database we allow the results to be edited without the need to reenter all the data and evaluation it once more and in case of a lost excel file, it can be recreated with a click of a button.

### Our exam creation process consists of:

1. Selecting an assignment
2. Creating an exam with a type and date
3. Based on the answer keys creating exam groups
4. For each question in the answer keys creating a question and assigning to it the wanted outcomes
5. For each question and student in a given exam group creating a result with the student’s answer

After finishing the design of the database, we started to design the program UI by sketching the wanted windows and panel with the basic functionality of each one. With the ending of the early designing of the system we started the research phase for the project which consisted of these parts:

1. **The Platform:**  According to our past skills and the given period of the project we decided to use the .Net platform on windows with C\# language to develop the project with more advanced programming techniques and design patterns.
2. **The UI Type:**  Due to the lack of UI scaling feature in windows form and the fact that it is using ancient technology for displaying content we decided to use WPF with MahApp.Metro framework to achieve a good looking, responsive and scalable UI with minimum effort in addition to the ability to design the UI separated from the code behind which gave us flexibility in making updates and improvement to the UI without the need to change the back-end side of the forms.
3. **Database:** As mentioned above we decided to go with C\# as the programming language for the project we didn’t find a better solution than MS SQL Serve to handle the database for the following reasons:
   * Being developed by Microsoft assures the compatibility and optimization for Windows
   * Better compatibility and speeds when working with C\#

