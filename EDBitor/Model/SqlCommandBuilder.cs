using System;
using System.Data.SQLite;
using System.Text;

namespace EDBitor.Model
{
    static class SqlCommandBuilder
    {
        public static SQLiteCommand Select(this SQLiteCommand command, params string[] columns)
        {
            var columnNames = columns == null ? "*" : string.Join(", ", columns);
            command.CommandText = $"select {columnNames} ";

            return command;
        }

        public static SQLiteCommand From(this SQLiteCommand command, string table)
        {
            command.CommandText = $"{command.CommandText} from {table} ";
            return command;
        }

        public static SQLiteCommand Where(this SQLiteCommand command, string conditionFormat, params object[] args)
        {
            var condition = string.Format(conditionFormat, args);
            command.CommandText = $"{command.CommandText} where {condition} ";
            return command;
        }

        public static SQLiteCommand InsertInto(this SQLiteCommand command, string table, params string[] columns)
        {
            command.CommandText = $"insert into {table} ({string.Join(", ", columns)}) ";
            return command;
        }

        public static SQLiteCommand Values(this SQLiteCommand command, params string[] values)
        {
            command.CommandText = $"{command.CommandText} values ({string.Join(", ", values)}) ";
            return command;
        }

        public static SQLiteCommand GetLastInsertedId(this SQLiteCommand command, params string[] values)
        {
            const string getLastInsertedIdCommand = "select last_insert_rowid();";
            command.CommandText = string.IsNullOrEmpty(command.CommandText) 
                ? $"{getLastInsertedIdCommand};"
                : $"{command.CommandText}; {getLastInsertedIdCommand};";

            return command;
        }

        public static SQLiteCommand Update(this SQLiteCommand command, string table)
        {
            command.CommandText = $"update {table} ";
            return command;
        }

        public static SQLiteCommand Set(this SQLiteCommand command, Tuple<string, string>[] values)
        {
            const int capacity = 128;
            var setList = new StringBuilder(capacity);
            for (int i = 0; i < values.Length; i++)
            {
                var kv = values[i];
                setList.AppendFormat("{0}={1}", kv.Item1, kv.Item2);
            }

            command.CommandText = $"{command.CommandText} set {setList} ";
            return command;
        }

        public static SQLiteCommand Delete(this SQLiteCommand command)
        {
            command.CommandText = "delete ";
            return command;
        }

        public static AsyncSQLiteCommand Async(this SQLiteCommand command)
        {
            return new AsyncSQLiteCommand(command);
        }
    }
}
