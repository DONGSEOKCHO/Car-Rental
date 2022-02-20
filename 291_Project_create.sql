USE[CMPT291_Project]

create table Customers(
	CustomerID int identity(1,1) Primary key,
	FirstName varchar(40) null,
	LastName varchar(40) null,
	DrivingLicense varchar(40) null,
	Membership varchar(40) null,
	StreetName varchar(40) null,
	StreetNumber varchar(40) null,
	AptNumber varchar(40) null,
	City varchar(40) null,
	Province varchar(40) null,
	Zip varchar(40) null,
	PhoneNumber varchar(40) null
)
create table Branches(
	Branch_ID int identity(1,1) primary key,
	street_name varchar(40) null,
	street_number varchar(40) null,
	city varchar(40) null,
	province varchar(40) null,
	zip varchar(8) null,
	phone_number varchar(10) null
	)

create table Car_types(
	CarType varchar(40) Primary key,
	Price_D float(40) null,
	Price_W float(40) null,
	Price_M float(40) null
	)

create table Cars(
	VIN nchar (40) Primary key,
	Color varchar(40) null,
	Model varchar(40) null,
	CarType varchar(40) not null,
	BranchID int not null,
	FOREIGN KEY (CarType) REFERENCES Car_types(CarType),
	FOREIGN KEY (BranchID) REFERENCES Branches(Branch_ID)
	)


create table Employees(
	Employee_ID int identity(1,1) primary key ,
	first_name varchar(40) null,
	middle_initial varchar(5) null,
	last_name varchar(40) null,
	street_name varchar(40) null,
	street_number varchar(40) null,
	city varchar(40) null,
	province varchar(40) null,
	zip varchar(8) null,
	phone_number varchar(10) null,
	Branch_ID int,
	FOREIGN KEY (Branch_ID) REFERENCES Branches(Branch_ID)
	)

create table Rental_Trans(
	Rental_ID int identity(1,1) primary key ,
	pickup_date date null,
	return_date date null,
	price float(40) null,
	Customer_ID int,
	Employee_ID int,
	pickup_Branch_ID int,
	return_Branch_ID int,
	VIN nchar(40),
	FOREIGN KEY (Customer_ID) REFERENCES Customers(CustomerID),
	FOREIGN KEY (Employee_ID) REFERENCES Employees(Employee_ID),
	FOREIGN KEY (pickup_Branch_ID) REFERENCES Branches(Branch_ID),
	FOREIGN KEY (return_Branch_ID) REFERENCES Branches(Branch_ID),
	FOREIGN KEY (VIN) REFERENCES Cars(VIN)
	)

insert into Customers values ('d','d','d','d','d','d','d','d','d','d','d')

insert into Branches values ( '149', '82 street', 'Edmonton', 'Alberta', 't5a0h6', '7803333333')

insert into Car_types values ('SUV', 45.50, 310.50, 1200.00)

insert into Cars values ('210834', 'Yellow', 'Elantra',
(select CarType from Car_types where CarType='SUV'),
(select Branch_ID from Branches where Branch_ID=1))

insert into Employees values ( 'Artorias', 'O', 'The Abyss', '31 street', '4152', 'Edmonton', 'Alberta', 't4l6v7', '7807807800',
(select Branch_ID from Branches where Branch_ID=1))

insert into Rental_Trans values ( '2020-11-27', '2020-12-27', 1200.00, 
(select CustomerID from Customers where CustomerID=1), 
(select Employee_ID from Employees where Employee_ID=1), 
(select Branch_ID from Branches where Branch_ID=1),
(select Branch_ID from Branches where Branch_ID=1),
(select VIN from Cars where VIN='210834'))
