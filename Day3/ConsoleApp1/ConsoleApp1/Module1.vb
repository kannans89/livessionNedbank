Module Module1
    Public Class Student
        Private _id As Integer
        Private _name As String
        Private _marks As Double

        ' Constructor to initialize the student details
        Public Sub New(id As Integer, name As String, marks As Double)
            _id = id
            _name = name
            _marks = marks
        End Sub

        ' Getter methods
        Public ReadOnly Property Id() As Integer
            Get
                Return _id
            End Get
        End Property

        Public ReadOnly Property Name() As String
            Get
                Return _name
            End Get
        End Property

        Public ReadOnly Property Marks() As Double
            Get
                Return _marks
            End Get
        End Property
    End Class

    Sub Main()
        ' Creating an instance of the Student class
        Dim student1 As New Student(1, "John Doe", 85.5)

        ' Accessing and printing student details
        Console.WriteLine("Student ID: " & student1.Id)
        Console.WriteLine("Student Name: " & student1.Name)
        Console.WriteLine("Student Marks: " & student1.Marks)

        ' To keep the console window open
        Console.ReadLine()
    End Sub
End Module
