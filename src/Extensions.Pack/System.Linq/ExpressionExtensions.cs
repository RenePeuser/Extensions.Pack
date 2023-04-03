using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Argument.Check;

namespace Extensions.Pack
{
    /// <summary>The extension class for expressions.</summary>
    public static class ExpressionExtensions
    {
        /// <summary>This method extract the property name of an expression.</summary>
        public static TResult? GetValueOfExpression<TResult>(this Expression expression)
        {
            Throw.IfNull(expression);

            return expression switch
            {
                MemberExpression memberExpression => memberExpression.GetValueOfExpression<TResult>(),
                ConstantExpression constantExpression => constantExpression.GetValueOfExpression<TResult>(),
                UnaryExpression unaryExpression => unaryExpression.GetValueOfExpression<TResult>(),
                MethodCallExpression methodCallExpression => methodCallExpression.GetValueOfExpression<TResult>(),
                _ => throw new InvalidOperationException("Unknown expression for extracting value")
            };
        }

        /// <summary>This method extract the property name of an expression.</summary>
        public static TResult? GetValueOfExpression<TResult>(this MethodCallExpression methodCallExpression)
        {
            return Expression.Lambda(methodCallExpression).Compile().DynamicInvoke().As<TResult>();
        }

        /// <summary>This method extract the property name of an expression.</summary>
        public static string? GetNameOfExpression(this Expression expression)
        {
            Throw.IfNull(expression);

            return expression switch
            {
                MemberExpression memberExpression => memberExpression.Member.Name,
                UnaryExpression unaryExpression => unaryExpression.Operand.As<MemberExpression>()?.Member.Name,
                _ => "Unknown"
            };
        }

        /// <summary>This method extract the type of the argument or return type of an method call expression.</summary>
        public static Type? GetTypeOfExpression(this Expression expression)
        {
            Throw.IfNull(expression);

            return expression switch
            {
                MemberExpression memberExpression => memberExpression.Member.Cast<FieldInfo>().FieldType,
                UnaryExpression unaryExpression => unaryExpression.Operand.As<MemberExpression>()?.Member.Cast<FieldInfo>().FieldType,
                MethodCallExpression methodCallExpression => methodCallExpression.Method.ReturnType,
                _ => null
            };
        }

        /// <summary>This method extract the property name of an expression.</summary>
        public static TResult? GetValueOfExpression<TResult>(this ConstantExpression constantExpression)
        {
            Throw.IfNull(constantExpression);

            return constantExpression.Value.As<TResult>();
        }

        /// <summary>This method extract the property name of an expression.</summary>
        public static TResult? GetValueOfExpression<TResult>(this UnaryExpression unaryExpression)
        {
            Throw.IfNull(unaryExpression);

            var memberExpression = unaryExpression.Operand.As<MemberExpression>();

            if (memberExpression.IsNotNull())
            {
                return memberExpression.GetValueOfExpression<TResult>();
            }

            throw new InvalidOperationException("Can not extract value of unary expression");
        }

        /// <summary>This method extract the property name of an expression.</summary>
        public static TResult? GetValueOfExpression<TResult>(this MemberExpression memberExpression)
        {
            Throw.IfNull(memberExpression);

            var constantExpression = memberExpression.Expression.As<ConstantExpression>();

            if (constantExpression.IsNull())
            {
                throw new ArgumentException("Expression is not a constant expression", nameof(memberExpression));
            }

            var fieldInfo = memberExpression.Member.As<FieldInfo>();

            if (fieldInfo.IsNull())
            {
                throw new ArgumentException("Member of member expression is not a field info", nameof(memberExpression));
            }

            var result = fieldInfo.GetValue(constantExpression.Value);

            return result.As<TResult>();
        }

        /// <summary>This method extract the property name of an expression.</summary>
        /// <typeparam name="T">Generic type of the property.</typeparam>
        /// <param name="propertyExpression">The property expression.</param>
        /// <returns>The name of the expected property.</returns>
        public static string ExtractPropertyName<T>(this Expression<Func<T>> propertyExpression)
        {
            Throw.IfNull(propertyExpression);

            var memberExpression = propertyExpression.Body.As<MemberExpression>();

            if (memberExpression.IsNull())
            {
                throw new ArgumentException("Expression is not a member expression", nameof(propertyExpression));
            }

            var propertyInfo = memberExpression.Member.As<PropertyInfo>();

            if (propertyInfo.IsNull())
            {
                throw new ArgumentException("Member of member expression is not a property info", nameof(propertyExpression));
            }

            var methodInfo = propertyInfo.GetGetMethod(true);
            Throw.IfNull(methodInfo);

            if (methodInfo.IsStatic)
            {
                throw new ArgumentException("Gets the accessors of the property info is static", nameof(propertyExpression));
            }

            return memberExpression.Member.Name;
        }

        /// <summary>Extracts the value of a specific member expression.</summary>
        /// <typeparam name="T">The generic type of the expected value.</typeparam>
        /// <param name="argumentExpression">The argument expression.</param>
        /// <returns>T.</returns>
        public static T? GetMethod<T>(this Expression<T> argumentExpression)
        {
            Throw.IfNull(argumentExpression);

            var methodCallExpression = argumentExpression.Body.As<MethodCallExpression>();

            if (methodCallExpression.IsNull())
            {
                throw new ArgumentException("Expression is not a member expression", nameof(argumentExpression));
            }

            return default;
        }

        /// <summary>Extracts the value of a specific member expression.</summary>
        /// <typeparam name="T">The generic type of the expected value.</typeparam>
        /// <param name="argumentExpression">The argument expression.</param>
        /// <returns>T.</returns>
        public static T? GetMemberValue<T>(this Expression<Func<T>> argumentExpression)
        {
            Throw.IfNull(argumentExpression);

            var memberExpression = argumentExpression.Body.As<MemberExpression>();

            if (memberExpression.IsNull())
            {
                throw new ArgumentException("Expression is not a member expression", nameof(argumentExpression));
            }

            var constantExpression = memberExpression.Expression.As<ConstantExpression>();

            if (constantExpression.IsNull())
            {
                throw new ArgumentException("Expression is not a constant expression", nameof(argumentExpression));
            }

            var fieldInfo = memberExpression.Member.As<FieldInfo>();

            if (fieldInfo.IsNull())
            {
                throw new ArgumentException("Member of member expression is not a field info", nameof(argumentExpression));
            }

            var result = fieldInfo.GetValue(constantExpression.Value);

            return result.As<T>();
        }

        /// <summary>Extracts the value of a specific member expression.</summary>
        /// <typeparam name="T">The generic type of the expected value.</typeparam>
        /// <param name="argumentExpression">The argument expression.</param>
        /// <returns>T.</returns>
        public static T? GetMemberValue<T>(this Expression<T> argumentExpression)
        {
            Throw.IfNull(argumentExpression);

            var memberExpression = argumentExpression.Body.As<MemberExpression>();

            if (memberExpression.IsNull())
            {
                throw new ArgumentException("Expression is not a member expression", nameof(argumentExpression));
            }

            var constantExpression = memberExpression.Expression.As<ConstantExpression>();

            if (constantExpression.IsNull())
            {
                throw new ArgumentException("Expression is not a constant expression", nameof(argumentExpression));
            }

            var fieldInfo = memberExpression.Member.As<FieldInfo>();

            if (fieldInfo.IsNull())
            {
                throw new ArgumentException("Member of member expression is not a field info", nameof(argumentExpression));
            }

            var result = fieldInfo.GetValue(constantExpression.Value);

            return result.As<T>();
        }

        /// <summary>This is a generic extension to get the name of a property or method and so on.</summary>
        /// <param name="expression">The expression from which we want to extract the expected name.</param>
        /// <returns>The name of the expected property method and so on.</returns>
        public static string NameOf(this Expression expression)
        {
            Throw.IfNull(expression);

            var lambdaExpression = expression.As<LambdaExpression>();

            if (lambdaExpression.IsNull())
            {
                throw new ArgumentException("Expression is not a LambdaExpression");
            }

            var name = "n.A";
            var memberExpression = lambdaExpression.Body.As<MemberExpression>();

            if (memberExpression != null)
            {
                name = memberExpression.Member.Name;
            }

            var unaryExpression = lambdaExpression.Body.As<UnaryExpression>();

            if (unaryExpression != null)
            {
                var member = unaryExpression.Operand.As<MemberExpression>();

                if (member != null)
                {
                    name = member.Member.Name;
                }
            }

            var methodCallExpression = lambdaExpression.Body.As<MethodCallExpression>();

            if (methodCallExpression != null)
            {
                name = methodCallExpression.Method.Name;
            }

            if (name == null)
            {
                throw new ArgumentException("Unknown expression type for extracting name.", nameof(expression));
            }

            return name;
        }

        /// <summary>Converts an expression array to a dictionary which contains as key the right hand operand name and as value the compiled expression.</summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="expressions">The expressions array.</param>
        /// <returns>A dictionary which contains as key the right hand operand name and as value the compiled expression.</returns>
        public static Dictionary<string, Func<T, object>> ToCompiledExpressionWithInfo<T>(this Expression<Func<T, object>>[] expressions)
        {
            Throw.IfNull(expressions);

            var result = expressions.ToDictionary(item => item.NameOf(), item => item.Compile());

            return result;
        }

        /// <summary>To the new array expression.</summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="expressions">The expressions.</param>
        /// <returns>NewArrayExpression.</returns>
        public static NewArrayExpression ToNewArrayExpressionOfType<T>(this IEnumerable<Expression> expressions)
        {
            var constantExpressions = expressions.Select(expr => Expression.Convert(expr, typeof(T))).ToList();

            return Expression.NewArrayInit(typeof(T), constantExpressions);
        }
    }
}
