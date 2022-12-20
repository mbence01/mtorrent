using System.Data;
using System.Data.SqlClient;

namespace Torrent.Service.Access.Common;

/// <summary>
/// Enum which describes the most currently available operators in SQL language.
/// </summary>
public enum Operator
{
	Equals,
	NotEquals,
	GreaterThan,
	GreaterThanOrEquals,
	LessThan,
	LessThanOrEquals,
	Like,
	IsNull,
	IsNotNull
}

/// <summary>
/// Class for WHERE clause representations in SQL queries with the usage of SQL parameters.
/// </summary>
public class SqlWhereClause
{
	private List<string> _fields;
	private List<Operator> _operators;
	private List<SqlParameter> _parameters;
	private string _conjunction;

	/// <summary>
	/// Initializes a new <see cref="SqlWhereClause"/> instance. If <paramref name="conjunction" /> is given, it initializes that too, unless it will be AND by default.
	/// </summary>
	/// <param name="conjunction">conjunction word, for example: AND, OR</param>
	public SqlWhereClause(string conjunction = "AND")
	{
		_conjunction = conjunction;

		_fields = new List<string>();
		_operators = new List<Operator>();
		_parameters = new List<SqlParameter>();
	}

	/// <summary>
	/// Private constructor for the <see cref="CreateEmpty"/> function. It does not initialize anything, all properties will remain null.
	/// </summary>
	/// <param name="empty"></param>
	private SqlWhereClause(bool empty) {}

	/// <summary>
	/// Creates a new <see cref="SqlWhereClause"/> instance with default NULL values for an empty WHERE clause.
	/// </summary>
	/// <returns></returns>
	public static SqlWhereClause CreateEmpty() => new SqlWhereClause(true);
	
	/// <summary>
	/// Adds a where condition to the list.
	/// </summary>
	/// <param name="fieldName">Field name in SQL table</param>
	/// <param name="op">Operator that is used, for example: =, !=</param>
	/// <param name="value">Value of <paramref name="fieldName"/> that should be checked</param>
	public SqlWhereClause AddCondition(string fieldName, Operator op, string value)
	{
		_fields.Add(fieldName);
		_parameters.Add(new SqlParameter(GetParameterName(fieldName), value) { DbType = DbType.String, Size = value.Length });
		_operators.Add(op);

		return this;
	}

	/// <summary>
	/// Initializes a new <see cref="SqlWhereClause"/> object from the given parameters. Useful when you only need to add one condition to the where clause.
	/// </summary>
	/// <param name="fieldName">Field name in SQL table</param>
	/// <param name="op">Operator that is used, for example: =, !=</param>
	/// <param name="value">Value of <paramref name="fieldName"/> that should be checked</param>
	/// <returns><see cref="SqlWhereClause"/> object created by the parameters</returns>
	public static SqlWhereClause From(string fieldName, Operator op, string value) 
		=> new SqlWhereClause().AddCondition(fieldName, op, value);

	/// <summary>
	/// Returns the currently saved SQL parameters.
	/// </summary>
	/// <returns>List of <see cref="SqlParameter"/></returns>
	public List<SqlParameter> GetParameters() => _parameters;

	/// <summary>
	/// Returns the string representation (which fits in a SQL query) of the conditions given so far. If no conditions were given, it returns an empty string.
	/// </summary>
	/// <returns>String representation of conditions</returns>
	public override String ToString()
	{
		if(_conjunction == null || _fields == null || _operators == null || _parameters == null)
			return String.Empty;
		
		return "WHERE " + String.Join(_conjunction,
			Enumerable.Range(0, _fields.Count)
				.Select(i => $"{_fields[i]} {GetStringForOperator(_operators[i])} {_parameters[i].ParameterName}"));
	}

	/// <summary>
	/// Returns a SQL parameter name based on the <see cref="fieldName"/> and the pre-defined string format algorythm.
	/// </summary>
	/// <param name="fieldName">Field name in SQL table</param>
	/// <returns>SQL parameter name</returns>
	private string GetParameterName(string fieldName)
	{
		return $"@mtrt_w_cls_{fieldName.ToLower().Replace(' ', '_')}";
	}

	/// <summary>
	/// Returns the string representation of an <see cref="Operator"/>.
	/// </summary>
	/// <param name="op">Operator</param>
	/// <returns>String representation</returns>
	private string GetStringForOperator(Operator op)
	{
		switch (op)
		{
			case Operator.Equals: return "=";
			case Operator.NotEquals: return "!=";
			case Operator.GreaterThan: return ">";
			case Operator.GreaterThanOrEquals: return ">=";
			case Operator.LessThan: return "<";
			case Operator.LessThanOrEquals: return "<=";
			case Operator.Like: return "LIKE";
			case Operator.IsNull: return "IS NULL";
			case Operator.IsNotNull: return "IS NOT NULL";
			default: return "";
		}
	}
}