using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DisputePortal.Persistence.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class PostgresExtensions
    {
        /// <summary>
        /// Convert the entity name to a table name.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="schema"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static EntityTypeBuilder<TEntity> ToTableName<TEntity>(this EntityTypeBuilder<TEntity> builder, string? schema = null) where TEntity : class
        {
            var type = typeof(TEntity);
            var tableName = type.Name.Replace("Model", "");

            return schema == null 
                ? builder.ToTable(tableName)
                : builder.ToTable(tableName, schema);
        }

        /// <summary>
        /// Set the column type to "timestamp" for the specified entity.
        /// </summary>
        /// <param name="builder"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static PropertyBuilder<TEntity> HasPostgresTimeStamp<TEntity>(this PropertyBuilder<TEntity> builder) 
            => builder.HasColumnType("timestamp");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static PropertyBuilder<TEntity> HasPostgresDefaultCurrentTimeStamp<TEntity>(this PropertyBuilder<TEntity> builder) 
            => builder.HasDefaultValueSql("CURRENT_TIMESTAMP");

        public static PropertyBuilder<TEntity> HasPostgresDefaultCurrentTimeStampTz<TEntity>(this PropertyBuilder<TEntity> builder) 
            => builder.HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

        /// <summary>
        /// Set the column type to "timestamptz" for the specified entity.
        /// </summary>
        /// <param name="builder"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static PropertyBuilder<TEntity> HasPostgresTimeStampTz<TEntity>(this PropertyBuilder<TEntity> builder)
            => builder.HasColumnType("timestamptz");

        /// <summary>
        /// Set the column type to "date" for the specified entity.
        /// </summary>
        /// <param name="builder"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static PropertyBuilder<TEntity> HasPostgresDateOnly<TEntity>(this PropertyBuilder<TEntity> builder) 
            => builder.HasColumnType("date");
        
        /// <summary>
        /// Set the column type to "time" for the specified entity.
        /// </summary>
        /// <param name="builder"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static PropertyBuilder<TEntity> HasPostgresTimeOnly<TEntity>(this PropertyBuilder<TEntity> builder) 
            => builder.HasColumnType("time");

        /// <summary>
        /// Set the column type to "uuid" for the specified entity.
        /// </summary>
        /// <param name="builder"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static PropertyBuilder<TEntity> HasPostgresUuid<TEntity>(this PropertyBuilder<TEntity> builder) 
        => builder.HasColumnType("uuid");

        public static PropertyBuilder<TEntity> HasPostgresJsonb<TEntity>(this PropertyBuilder<TEntity> builder) 
        => builder.HasColumnType("jsonb");

        /// <summary>
        /// Set the column type to "json" for the specified entity.
        /// </summary>
        /// <param name="builder"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static PropertyBuilder<TEntity> HasPostgresJson<TEntity>(this PropertyBuilder<TEntity> builder) 
        => builder.HasColumnType("json");

        /// <summary>
        /// Set the column type to "hstore" for the specified entity.
        /// </summary>
        /// <param name="builder"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static PropertyBuilder<TEntity> HasPostgresHStore<TEntity>(this PropertyBuilder<TEntity> builder) 
        => builder.HasColumnType("hstore");

        /// <summary>
        /// Set the column type to "Array" for the specified entity.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="type"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static PropertyBuilder<TEntity> HasPostgresArray<TEntity>(this PropertyBuilder<TEntity> builder, string type) 
        => builder.HasColumnType($"{type}[]");

        /// <summary>
        /// Set the column type to "varchar" for the specified entity.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="length">The maximum length of the varchar </param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static PropertyBuilder<TEntity> HasPostgresVarChar<TEntity>(this PropertyBuilder<TEntity> builder, int length) 
            => builder.HasColumnType($"varchar({length})");

        /// <summary>
        /// Set the column type to "text" for the specified entity.
        /// </summary>
        /// <param name="builder"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static PropertyBuilder<TEntity> HasPostgresText<TEntity>(this PropertyBuilder<TEntity> builder)  => builder.HasColumnType("text");

        /// <summary>
        /// Set the column type to "char" for the specified entity.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="length"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static PropertyBuilder<TEntity> HasPostgresChar<TEntity>(this PropertyBuilder<TEntity> builder, int length) 
        => builder.HasColumnType($"char({length})");

        /// <summary>
        /// Set the column type to "boolean" for the specified entity.
        /// </summary>
        /// <param name="builder"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static PropertyBuilder<TEntity> HasPostgresBoolean<TEntity>(this PropertyBuilder<TEntity> builder) => builder.HasColumnType("boolean");

        /// <summary>
        /// Set the column type to "numeric" for the specified entity.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="precision">  </param>
        /// <param name="scale"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static PropertyBuilder<TEntity> HasPostgresNumeric<TEntity>(this PropertyBuilder<TEntity> builder, int precision, int scale) 
        => builder.HasColumnType($"numeric({precision}, {scale})");

        /// <summary>
        /// Set the column type to "decimal" for the specified entity.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="precision"></param>
        /// <param name="scale"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static PropertyBuilder<TEntity> HasPostgresDecimal<TEntity>(this PropertyBuilder<TEntity> builder, int precision, int scale) 
        => builder.HasColumnType($"decimal({precision}, {scale})");

        /// <summary>
        /// Set the column type to "money" for the specified entity.
        /// </summary>
        /// <param name="builder"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static PropertyBuilder<TEntity> HasPostgresMoney<TEntity>(this PropertyBuilder<TEntity> builder) 
        => builder.HasColumnType("money");

        /// <summary>
        /// Set the column type to "smallint" for the specified entity.
        /// </summary>
        /// <param name="builder"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static PropertyBuilder<TEntity> HasPostgresSmallInt<TEntity>(this PropertyBuilder<TEntity> builder) 
        => builder.HasColumnType("smallint");

        /// <summary>
        /// Set the column type to "integer" for the specified entity.
        /// </summary>
        /// <param name="builder"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static PropertyBuilder<TEntity> HasPostgresInteger<TEntity>(this PropertyBuilder<TEntity> builder) 
        => builder.HasColumnType("integer");

        /// <summary>
        /// Set the column type to "bigint" for the specified entity.
        /// </summary>
        /// <param name="builder"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static PropertyBuilder<TEntity> HasPostgresBigInt<TEntity>(this PropertyBuilder<TEntity> builder) 
        => builder.HasColumnType("bigint");

        /// <summary>
        /// Set the column type to "real" for the specified entity.
        /// </summary>
        /// <param name="builder"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static PropertyBuilder<TEntity> HasPostgresReal<TEntity>(this PropertyBuilder<TEntity> builder) 
        => builder.HasColumnType("real");

        /// <summary>
        /// Set the column type to "double precision" for the specified entity.
        /// </summary>
        /// <param name="builder"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static PropertyBuilder<TEntity> HasPostgresDoublePrecision<TEntity>(this PropertyBuilder<TEntity> builder) 
        => builder.HasColumnType("double precision");
    }
}
