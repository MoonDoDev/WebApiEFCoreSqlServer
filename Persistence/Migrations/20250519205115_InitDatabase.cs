﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
	/// <inheritdoc />
	public partial class InitDatabase : Migration
	{
		/// <inheritdoc />
		protected override void Up( MigrationBuilder migrationBuilder )
		{
			migrationBuilder.CreateTable(
				name: "Employees",
				columns: table => new
				{
					Id = table.Column<Guid>( type: "uniqueidentifier", nullable: false ),
					Name = table.Column<string>( type: "nvarchar(100)", maxLength: 100, nullable: false ),
					Position = table.Column<string>( type: "nvarchar(50)", maxLength: 50, nullable: false ),
					Salary = table.Column<decimal>( type: "decimal(18,2)", nullable: false )
				},
				constraints: table =>
				{
					table.PrimaryKey( "PK_Employees", x => x.Id );
				} );
		}

		/// <inheritdoc />
		protected override void Down( MigrationBuilder migrationBuilder )
		{
			migrationBuilder.DropTable(
				name: "Employees" );
		}
	}
}
