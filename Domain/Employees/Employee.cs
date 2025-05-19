using System.ComponentModel.DataAnnotations;

namespace Domain.Employees;

public record Employee
{
	[Key]
	public Guid Id { get; private set; }
	public string Name { get; private set; } = string.Empty;
	public string Position { get; private set; } = string.Empty;
	public decimal Salary { get; private set; }

	private Employee() { }

	public Employee( Guid id, string name, string position, decimal salary )
	{
		this.Id = id;
		this.Name = name;
		this.Position = position;
		this.Salary = salary;
	}

	public void Update( string name, string position, decimal salary )
	{
		this.Name = name;
		this.Position = position;
		this.Salary = salary;
	}
}
