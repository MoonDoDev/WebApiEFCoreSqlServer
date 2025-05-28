using System.ComponentModel.DataAnnotations;

namespace Domain.Employees;

public sealed record Employee
{
	[Key]
	public Guid Id { get; private set; }
	public string Name { get; private set; } = string.Empty;
	public string Position { get; private set; } = string.Empty;
	public decimal Salary { get; private set; }

	private Employee() { }

	public Employee( Guid id, string name, string position, decimal salary )
	{
		Id = id;
		Name = name;
		Position = position;
		Salary = salary;
	}

	public void Update( string name, string position, decimal salary )
	{
		Name = name;
		Position = position;
		Salary = salary;
	}
}
