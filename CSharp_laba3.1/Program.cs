using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Design;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;

namespace CSharp_laba3._1
{
    class StudentContext: DbContext
    {
        public StudentContext() : base("DBConnection") { }
        public DbSet<Student> Students { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Activity> Activities { get; set; }
    }
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public int? GroupId { get; set; } 
        public Group Group { get; set; }
        public ICollection<Activity> Activities { get; set; }
        public Student()
        {
            Activities = new List<Activity>();
        }

    }

    public class Group
    {
        public int Id { get; set; }
        public string NameOfTeacher { get; set; }
        public ICollection<Student> Students { get; set; }

    }

    public class Activity
    {
        public int Id { get; set; }
        public string nameOfActivity { get; set; }
        public ICollection<Student> Students { get; set; }
        public Activity()
        {
            Students = new List<Student>();
        }
    }

    internal class Program
    {
        static public void Menu()
        {
            Console.WriteLine("Веберите таблицу:");
            Console.WriteLine("1 - Студенты");
            Console.WriteLine("2 - Группы");
            Console.WriteLine("3 - Студенческие группы");
            Console.WriteLine("0 - Завершение работы");
        }

        static public void MenuForTables()
        {
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("Create - создать");
            Console.WriteLine("Read - вывести таблицу");
            Console.WriteLine("Update - изменить значение в строке");
            Console.WriteLine("Delete - удалить строку");
        }
        
        static void Main(string[] args)
        {
            using (var db = new StudentContext())
            {
                db.SaveChanges();
                
                string move;
                string table;
                bool flag = true;

                while(flag)
                {
                    Program.Menu();
                    table = Console.ReadLine();
                    while(table != 1.ToString() && table != 2.ToString() && table != 0.ToString() && table != 3.ToString())
                    {
                        Console.WriteLine("Введите корректный номер таблицы");
                        table = Console.ReadLine();
                    }
                    switch (table) 
                    {
                        case "1":
                            Program.MenuForTables();
                            move = Console.ReadLine();
                            while(move != "Create" && move != "Read" && move != "Update" && move != "Delete")
                            {
                                Console.WriteLine("Введите корректное действие");
                                move = Console.ReadLine();
                            }
                            switch (move) {
                                case "Create":
                                    Console.WriteLine("Введите параметры студента: имя, фамилия, возраст");
                                    string name = Console.ReadLine();
                                    string surname = Console.ReadLine();
                                    int age = System.Convert.ToInt32(Console.ReadLine());
                                    Student s = new Student { Name = name, Surname = surname, Age = age };
                                    db.Students.Add(s);
                                    db.SaveChanges();
                                    Console.WriteLine();
                                    break;
                                case "Read":
                                    Console.WriteLine("List of students:");
                                    foreach (Student st in db.Students.Include(st => st.Activities))
                                    {
                                        Console.WriteLine("{0}.{1} {2} {4} группа - {3}", st.Id, st.Name, st.Surname, st.Age, st.GroupId);
                                        Console.WriteLine("Список студенческих групп, в которые входит студент:");
                                        foreach(Activity a in st.Activities)
                                        {
                                            Console.WriteLine("- {0}", a.nameOfActivity);
                                        }
                                    }
                                    Console.WriteLine();
                                    break;
                                case "Update":
                                    Console.WriteLine("Введите номер студента");
                                    int number = System.Convert.ToInt32(Console.ReadLine());
                                    while(db.Students.Find(number) == null)
                                    {
                                        Console.WriteLine("Введите корректный номер студента");
                                        number = System.Convert.ToInt32(Console.ReadLine());
                                    }

                                    Student student = db.Students.Find(number);
                                    Console.WriteLine("Введите параметры, которые хотите изменить: имя, фамилия, возраст");
                                    name = Console.ReadLine();
                                    surname = Console.ReadLine();
                                    string sAge = Console.ReadLine();
                                    
                                    if (student != null)
                                    {
                                        if(name.Length > 0)
                                            student.Name = name;

                                        if (surname.Length > 0)
                                            student.Surname = surname;

                                        if (sAge.Length > 0)
                                            student.Age = System.Convert.ToInt32(sAge);

                                        db.Entry(student).State = EntityState.Modified;
                                        db.SaveChanges();
                                    }
                                    Console.WriteLine();
                                    break;
                                case "Delete":
                                    Console.WriteLine("Введите номер студента");
                                    number = System.Convert.ToInt32(Console.ReadLine());
                                    while (db.Students.Find(number) == null)
                                    {
                                        Console.WriteLine("Введите корректный номер студента");
                                        number = System.Convert.ToInt32(Console.ReadLine());
                                    }

                                    student = db.Students.Find(number);
                                    foreach (var a in student.Activities)
                                    {
                                        a.Students.Remove(student);
                                    }
                                    db.Students.Remove(student);
                                    db.SaveChanges();
                                    Console.WriteLine();
                                    break;
                            }
                            break;
                        case "2":
                            Program.MenuForTables();
                            move = Console.ReadLine();
                            while (move != "Create" && move != "Read" && move != "Update" && move != "Delete")
                            {
                                Console.WriteLine("Введите корректное действие");
                                move = Console.ReadLine();
                            }
                            switch (move)
                            {
                                case "Create":
                                    Console.WriteLine("Введите имя куратора группы");
                                    int number = -1;
                                    string teacher = Console.ReadLine();

                                    Group group = new Group { NameOfTeacher = teacher};
                                    db.Groups.Add(group);
                                    db.SaveChanges();

                                    Console.WriteLine("Добавить учеников?");
                                    string isStudents;
                                    isStudents = Console.ReadLine();
                                    while(isStudents != "Yes" && isStudents != "No")
                                    {
                                        Console.WriteLine("Введите Yes или No");
                                        isStudents= Console.ReadLine();
                                    }

                                    if(isStudents == "Yes")
                                    {
                                        Console.WriteLine("Выберите номер или номера учеников, 0 - стоп");
                                        var students = db.Students;
                                        Console.WriteLine("List of students:");
                                        foreach (Student st in students)
                                        {
                                            Console.WriteLine("{0}.{1} {2} - {3}", st.Id, st.Name, st.Surname, st.Age);
                                        }
                                        while(number != 0)
                                        {
                                            number = System.Convert.ToInt32(Console.ReadLine());
                                            if (db.Students.Find(number) == null && number != 0)
                                            {
                                                Console.WriteLine("Студента с таким номером не существует, введите номер повторно");
                                                number = System.Convert.ToInt32(Console.ReadLine());
                                            }
                                            else if (number != 0)
                                            {
                                                Student student;
                                                student = db.Students.Find(number);
                                                student.Group = group;
                                                student.GroupId = group.Id;
                                                db.SaveChanges();
                                            }
                                        }
                                        
                                    }
                                    break;
                                case "Read":
                                    foreach (Group g in db.Groups.Include(g => g.Students))
                                    {
                                        Console.WriteLine("Группа номер {0}, куратор {1}", g.Id, g.NameOfTeacher);
                                        Console.WriteLine("Список студентов:");
                                        foreach (var s in g.Students)
                                        {
                                            Console.WriteLine("{0} {1} - {2}", s.Name, s.Surname, s.Age);
                                        }
                                        Console.WriteLine();
                                    } 
                                    break;
                                case "Update":
                                    Console.WriteLine("Введите номер группы");
                                    number = System.Convert.ToInt32(Console.ReadLine());
                                    while (db.Groups.Find(number) == null)
                                    {
                                        Console.WriteLine("Введите корректный номер группы");
                                        number = System.Convert.ToInt32(Console.ReadLine());
                                    }
                                    group = db.Groups.Find(number);
                                    Console.WriteLine("Добавить учеников (Add), убрать ученика (Remove) или изменить куратора группы (Change)");
                                    move = Console.ReadLine();
                                    while(move != "Add" && move != "Change" && move != "Remove")
                                    {
                                        Console.WriteLine("Введите корректное действие");
                                        move = Console.ReadLine();
                                    }
                                    if (move == "Add")
                                    {
                                        Console.WriteLine("Выберите номер или номера учеников, 0 - стоп");
                                        var students = db.Students;
                                        Console.WriteLine("List of students:");
                                        foreach (Student st in students)
                                        {
                                            Console.WriteLine("{0}.{1} {2} - {3}", st.Id, st.Name, st.Surname, st.Age);
                                        }
                                        while (number != 0)
                                        {
                                            number = System.Convert.ToInt32(Console.ReadLine());
                                            if (db.Students.Find(number) == null && number != 0)
                                            {
                                                Console.WriteLine("Студента с таким номером не существует, введите номер повторно");
                                                number = System.Convert.ToInt32(Console.ReadLine());
                                            }
                                            else if (number != 0)
                                            {
                                                Student student;
                                                student = db.Students.Find(number);
                                                student.Group = group;
                                                student.GroupId = group.Id;
                                                db.SaveChanges();
                                            }
                                        }
                                    }
                                    else if (move == "Remove")
                                    {
                                        Console.WriteLine("Выберите номер или номера учеников, 0 - стоп");
                                        var students = group.Students;
                                        Console.WriteLine("List of students:");
                                        foreach (Student st in students)
                                        {
                                            Console.WriteLine("{0}.{1} {2} - {3}", st.Id, st.Name, st.Surname, st.Age);
                                        }
                                        while (number != 0)
                                        {
                                            number = System.Convert.ToInt32(Console.ReadLine());
                                            if (db.Students.Find(number) == null && number != 0)
                                            {
                                                Console.WriteLine("Студента с таким номером не существует, введите номер повторно");
                                                number = System.Convert.ToInt32(Console.ReadLine());
                                            }
                                            else if (number != 0)
                                            {
                                                Student student;
                                                student = db.Students.Find(number);
                                                student.Group = null;
                                                db.SaveChanges();
                                            }
                                        }

                                    }
                                    else
                                    {
                                        Console.WriteLine("Введите имя нового куратора");
                                        teacher = Console.ReadLine();
                                        group.NameOfTeacher = teacher;
                                    }
                                    break;
                                case "Delete":
                                    Console.WriteLine("Введите номер группы");
                                    number = System.Convert.ToInt32(Console.ReadLine());
                                    while (db.Groups.Find(number) == null)
                                    {
                                        Console.WriteLine("Введите корректный номер группы");
                                        number = System.Convert.ToInt32(Console.ReadLine());
                                    }
                                    group = db.Groups.Find(number);
                                    if (group != null)
                                    {
                                        foreach (var s in group.Students)
                                        {
                                            s.Group = null;
                                        }
                                        db.Groups.Remove(group);
                                        db.SaveChanges();
                                    }
                                    break;
                            }
                            break;
                        case "3":
                            Program.MenuForTables();
                            move = Console.ReadLine();
                            while (move != "Create" && move != "Read" && move != "Update" && move != "Delete")
                            {
                                Console.WriteLine("Введите корректное действие");
                                move = Console.ReadLine();
                            }
                            switch (move) {
                                case "Create":
                                    Console.WriteLine("Введите параметры студенческой группы: название");
                                    string name;
                                    name = Console.ReadLine();
                                    Activity act = new Activity { nameOfActivity = name };
                                    Console.WriteLine("Добавить учеников?");
                                    string yesOrNo;
                                    yesOrNo = Console.ReadLine();
                                    int number = -1;
                                    while(yesOrNo != "Yes" && yesOrNo != "No")
                                    {
                                        Console.WriteLine("Введите Tes или No");
                                        yesOrNo = Console.ReadLine();
                                    }
                                    if (yesOrNo == "Yes")
                                    {
                                        Console.WriteLine("Выберите номер или номера учеников, 0 - стоп");
                                        var students = db.Students;
                                        Console.WriteLine("List of students:");
                                        foreach (Student st in students)
                                        {
                                            Console.WriteLine("{0}.{1} {2} - {3}", st.Id, st.Name, st.Surname, st.Age);
                                        }
                                        while (number != 0)
                                        {
                                            number = System.Convert.ToInt32(Console.ReadLine());
                                            if (db.Students.Find(number) == null && number != 0)
                                            {
                                                Console.WriteLine("Студента с таким номером не существует, введите номер повторно");
                                                number = System.Convert.ToInt32(Console.ReadLine());
                                            }
                                            else if (number != 0)
                                            {
                                                Student student;
                                                student = db.Students.Find(number);
                                                act.Students.Add(student);
                                                student.Activities.Add(act);
                                            }
                                        }
                                    }
                                    db.Activities.Add(act);
                                    db.SaveChanges();
                                    break;
                                case "Read":
                                    foreach (Activity a in db.Activities.Include(a => a.Students))
                                    {
                                        Console.WriteLine("{0}. {1}", a.Id, a.nameOfActivity);
                                        Console.WriteLine("Список студентов:");
                                        foreach (var s in a.Students)
                                        {
                                            Console.WriteLine("{0} {1} - {2}", s.Name, s.Surname, s.Age);
                                        }
                                        Console.WriteLine();
                                    }
                                    break;
                                case "Update":
                                    Console.WriteLine("Введите id студенческой группы");
                                    number = System.Convert.ToInt32(Console.ReadLine());
                                    while(db.Activities.Find(number) == null)
                                    {
                                        Console.WriteLine("Студенческой группы с таким id не существует, введите корректный id");
                                        number = System.Convert.ToInt32(Console.ReadLine());
                                    }
                                    act = db.Activities.Find(number);
                                    Console.WriteLine("Выберите действие: Изменить название (Change), убрать студента из студенческой группы (Remove),");
                                    Console.WriteLine(" добавить студента в студенческую группу (Add)");
                                    move = Console.ReadLine();
                                    while(move != "Change" && move != "Remove" && move != "Add")
                                    {
                                        Console.WriteLine("Введено некорректное действие, введите еще раз");
                                        move = Console.ReadLine();
                                    }
                                    if (move == "Add")
                                    {
                                        Console.WriteLine("Выберите номер или номера учеников, 0 - стоп");
                                        var students = db.Students;
                                        Console.WriteLine("List of students:");
                                        foreach (Student st in students)
                                        {
                                            Console.WriteLine("{0}.{1} {2} - {3}", st.Id, st.Name, st.Surname, st.Age);
                                        }
                                        while (number != 0)
                                        {
                                            number = System.Convert.ToInt32(Console.ReadLine());
                                            if (db.Students.Find(number) == null && number != 0)
                                            {
                                                Console.WriteLine("Студента с таким номером не существует, введите номер повторно");
                                                number = System.Convert.ToInt32(Console.ReadLine());
                                            }
                                            else if (number != 0)
                                            {
                                                Student student;
                                                student = db.Students.Find(number);
                                                student.Activities.Add(act);
                                                act.Students.Add(student);
                                                db.SaveChanges();
                                            }
                                        }
                                    }
                                    else if (move == "Remove")
                                    {
                                        Console.WriteLine("Выберите номер студента");
                                        Console.WriteLine("Список студентов:");
                                        foreach (var s in act.Students)
                                        {
                                            Console.WriteLine("{0}.{1} {2} {4} группа - {3}", s.Id, s.Name, s.Surname, s.Age, s.GroupId);
                                        }
                                        Console.WriteLine();
                                        number = System.Convert.ToInt32(Console.ReadLine());
                                        while (db.Students.Find(number) == null)
                                        {
                                            Console.WriteLine("Студента с таким номером нет в группе, введите номер еще раз");
                                            number = System.Convert.ToInt32(Console.ReadLine());
                                        }
                                        Student student = db.Students.Find(number);
                                        student.Activities.Remove(act);
                                        act.Students.Remove(student);
                                        db.SaveChanges();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Введите новое название студенческой группы");
                                        name = Console.ReadLine();
                                        act.nameOfActivity = name;
                                        db.SaveChanges();
                                    }
                                    break;
                                case "Delete":
                                    Console.WriteLine("Введите номер студенческой группы");
                                    number = System.Convert.ToInt32(Console.ReadLine());
                                    while (db.Activities.Find(number) == null)
                                    {
                                        Console.WriteLine("Введите корректный номер студенческой группы");
                                        number = System.Convert.ToInt32(Console.ReadLine());
                                    }
                                    act = db.Activities.Find(number);
                                    if (act != null)
                                    {
                                        foreach (var s in act.Students)
                                        {
                                            s.Activities.Remove(act);
                                        }
                                        db.Activities.Remove(act);
                                        db.SaveChanges();
                                    }
                                    break;
                            }
                            break;
                        case "0":
                            flag = false;
                            break;
                    }

                }

                //DELETE
                //for (int i = 16; i > 7; i--)
                //{
                //    Student s = db.Students.Find(i);
                //    if(s != null)
                //    {
                //        db.Students.Remove(s);
                //        db.SaveChanges();
                //    }
                //}
                
                //UPDATE
                //Student studentFirst = db.Students.FirstOrDefault();
                //if (studentFirst != null)
                //{
                //    studentFirst.Name = "Cardy";
                //    studentFirst.Surname = "Bellington";
                //    studentFirst.Age = 20;

                //    db.Entry(studentFirst).State = EntityState.Modified;
                //    db.SaveChanges();
                //}
                
                //INSERT STUDENTS+GROUPS (ONE TO MANY)
                //Group group1 = new Group { NameOfTeacher = "Antony Fallen" };
                //Group group2 = new Group { NameOfTeacher = "Pomella Brooklyn" };
                
                //db.Groups.Add(group1);
                //db.Groups.Add(group2);
                //db.SaveChanges();

                //Student s1 = new Student { Name = "Roxane", Surname = "Blow", Age = 22, Group = group1 };
                //Student s2 = new Student { Name = "Maddy", Surname = "Fox", Age = 18, Group = group1 };
                //Student s3 = new Student { Name = "Bill", Surname = "Torney", Age = 20, Group = group2 };

                //db.Students.AddRange(new List <Student> { s1, s2, s3 });
                //db.SaveChanges();

                //foreach (Student s in db.Students.Include(p => p.Group))
                //    Console.WriteLine("{0} - {1}", s.Name, s.Group != null ? s.Group.NameOfTeacher : "");
                //Console.WriteLine();
                //foreach (Group g in db.Groups.Include(g => g.Students))
                //{
                //    Console.WriteLine("Группа, курирующаяся: {0}", g.NameOfTeacher);
                //    foreach (var s in g.Students)
                //    {
                //        Console.WriteLine("{0} {1} - {2}", s.Name, s.Surname, s.Age);
                //    }
                //    Console.WriteLine();
                //} 


                //INSERT STUDENTS
                //Student student1 = new Student { Name = "Alice" , Surname = "Browm", Age = 19};
                //Student student2 = new Student { Name = "Bob", Surname = "Duglas", Age = 21};

                //db.Students.Add(student1);
                //db.Students.Add(student2);
                //db.SaveChanges();
                //Console.WriteLine("Successfully");

                //var students = db.Students;
                //Console.WriteLine("List of students:");
                //foreach (Student s in students)
                //{
                //    Console.WriteLine("{0}.{1} {2} - {3}", s.Id, s.Name, s.Surname, s.Age);
                //}
            }
        }
    }
}
