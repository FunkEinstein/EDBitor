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
            command.CommandText = string.Format("select {0} ", columnNames);

            return command;
        }

        public static SQLiteCommand From(this SQLiteCommand command, string table)
        {
            command.CommandText = string.Format("{0} from {1} ", command.CommandText, table);
            return command;
        }

        public static SQLiteCommand Where(this SQLiteCommand command, string conditionFormat, params object[] args)
        {
            var condition = string.Format(conditionFormat, args);
            command.CommandText = string.Format("{0} where {1} ", command.CommandText, condition);
            return command;
        }

        public static SQLiteCommand InsertInto(this SQLiteCommand command, string table, params string[] columns)
        {
            command.CommandText = string.Format("insert into {0} ({1}) ", table, string.Join(", ", columns));
            return command;
        }

        public static SQLiteCommand Values(this SQLiteCommand command, params string[] values)
        {
            command.CommandText = string.Format("{0} values ({1}) ", command.CommandText, string.Join(", ", values));
            return command;
        }

        public static SQLiteCommand Update(this SQLiteCommand command, string table)
        {
            command.CommandText = string.Format("update {0} ", table);
            return command;
        }

        public static SQLiteCommand Set(this SQLiteCommand command, Tuple<string, string>[] values)
        {
            const int capacity = 128;
            StringBuilder setList = new StringBuilder(capacity);
            for (int i = 0; i < values.Length; i++)
            {
                var kv = values[i];
                setList.AppendFormat("{0}={1}", kv.Item1, kv.Item2);
            }

            command.CommandText = string.Format("{0} set {1} ", command.CommandText, setList);
            return command;
        }

        public static SQLiteCommand Delete(this SQLiteCommand command)
        {
            command.CommandText = "delete ";
            return command;
        }
    }
}
