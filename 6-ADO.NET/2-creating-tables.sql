USE Lab6db
GO

CREATE TABLE student
(
	id int PRIMARY KEY,
	fname varchar(30) NOT NULL,
	lname varchar(30) NOT NULL
)

CREATE TABLE wykladowca
(
	id int PRIMARY KEY,
	fname varchar(30) NOT NULL,
	lname varchar(30) NOT NULL
)

CREATE TABLE przedmiot
(
	id int PRIMARY KEY,
	name varchar(50) NOT NULL
)
GO

CREATE TABLE grupa
(
	id_wykl int REFERENCES wykladowca(id),
	id_stud int REFERENCES student(id),
	id_przed int REFERENCES przedmiot(id),
	PRIMARY KEY(id_wykl, id_stud, id_przed)
)

