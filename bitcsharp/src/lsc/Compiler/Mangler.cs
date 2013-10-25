using System.Text;
using LLVMSharp.Compiler.Ast;

namespace LLVMSharp.Compiler
{
    public class Mangler
    {
        private static Mangler _instance;

        public static Mangler Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Mangler();
                return _instance;
            }
        }

        public virtual string MapPrimitivesToFullQualifiedName(string n)
        {
            switch (n)
            {
                case "void":
                    return "System.Void";
                case "int":
                    return "System.Int32";
                case "float":
                    return "System.Single";
                case "char":
                    return "System.Char";
                case "string":
                    return "System.String";
                case "bool":
                    return "System.Boolean";
                case "object":
                    return "System.Object";
                default:
                    return string.Empty;
            }
        }

        public virtual string MangleName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return string.Empty;

            string[] s = name.Split('.');
            var sb = new StringBuilder();
            foreach (string item in s)
            {
                sb.Append(item.Length);
                sb.Append(item);
            }
            return sb.ToString();
        }

        protected virtual int ExtractNumber(string str)
        {
            int digit;
            int i;
            for (i = 0; i < str.Length; i++)
            {
                if (!int.TryParse(str[i].ToString(), out digit))
                    break;
            }

            return int.Parse(str.Substring(0, i));
        }

        public virtual string DemangleName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return string.Empty;

            var sb = new StringBuilder();

            bool start = true;
            while (name.Length > 0)
            {
                if (start)
                    start = false;
                else
                    sb.Append(".");

                int n, nLength;
                n = ExtractNumber(name);
                nLength = n.ToString().Length;

                string tmp = name.Substring(nLength, n);
                sb.Append(tmp);
                name = name.Substring(nLength + tmp.Length, name.Length - nLength - tmp.Length);
            }

            return sb.ToString();
        }

        // type=defaulted to System.Int32
        public virtual string MangleEnumMember(string ns, string enumName, string enumMemberName, string type)
        {
            var sb = new StringBuilder();
            sb.Append("Ei32_");
            sb.Append(MangleName(ns));
            sb.Append(MangleName(enumName));
            sb.Append(MangleName(enumMemberName));

            return sb.ToString();
        }

        public virtual string MangleEnumMember(string fullEnumtype, string enumMember)
        {
            var sb = new StringBuilder();
            sb.Append("Ei32_");
            sb.Append(MangleName(fullEnumtype));
            sb.Append(MangleName(enumMember));

            return sb.ToString();
        }

        public virtual string MangleMethod(AstMethod astMethod, bool isInherited)
        {
            string firstPortion;
            string SecondPortion;
            var temp1 = new StringBuilder();
            var sb = new StringBuilder();
            if (isInherited)
            {
                sb.Append("_mb_");
            }
            else
            {
                sb.Append("_mt_");
            }
            sb.Append(MangleName(astMethod.Name));
            foreach (AstParameter item in astMethod.Parameters)
            {
                string portion = item.FullQualifiedType; //null reference exception error
                if (item.FullQualifiedType != null && item.FullQualifiedType != item.Type)
                {
                    firstPortion = portion.Substring(0, portion.LastIndexOf('.'));
                    SecondPortion = portion.Substring((portion.LastIndexOf('.') + 1));

                    int paramLength1 = firstPortion.Length;
                    int paramLength2 = SecondPortion.Length;

                    temp1.Append(paramLength1);
                    temp1.Append(firstPortion);
                    temp1.Append(paramLength2);
                    temp1.Append(SecondPortion);

                    sb.Append("_" + temp1.Length + "_");

                    sb.Append(paramLength1);
                    sb.Append(firstPortion);
                    sb.Append(paramLength2);
                    sb.Append(SecondPortion);
                }
                else if(item.Type == item.FullQualifiedType)
                {
                    sb.Append(item.Type.Length);
                    sb.Append(item.Type);
                }
            }
            return sb.ToString();
        }

        //public virtual string MangleMethodForCodeGen(string mangleMethodNameInMethodTable, string astTypeFQT)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append(MangleName(astTypeFQT));
        //    sb.Append("_");
        //}

        public virtual string MangleMethod(string fullType, AstConstructor astConstructor, bool isInherited)
        {
            string firstPortion;
            string SecondPortion;
            var temp1 = new StringBuilder();
            var sb = new StringBuilder();
            if (isInherited)
            {
                sb.Append("_cb_");
            }
            else
            {
                sb.Append("_ct_");
            }


            sb.Append(MangleName(astConstructor.Name));
            
            foreach (AstParameter item in astConstructor.Parameters)
            {
                if (item.Type == item.FullQualifiedType)
                {
                    sb.Append(item.Type.Length);
                    sb.Append(item.Type);
                }
                else
                {
                    string portion = item.FullQualifiedType;
                    firstPortion = portion.Substring(0, portion.LastIndexOf('.'));
                    SecondPortion = portion.Substring((portion.LastIndexOf('.') + 1));

                    int paramLength1 = firstPortion.Length;
                    int paramLength2 = SecondPortion.Length;

                    temp1.Append(paramLength1);
                    temp1.Append(firstPortion);
                    temp1.Append(paramLength2);
                    temp1.Append(SecondPortion);

                    sb.Append("_" + temp1.Length + "_");

                    sb.Append(paramLength1);
                    sb.Append(firstPortion);
                    sb.Append(paramLength2);
                    sb.Append(SecondPortion);
                }
            }
            return sb.ToString();
        }


        public virtual string MangleAccessor(AstAccessor astAccessor, bool isInherited)
        {
            var sb = new StringBuilder();
            // sb.Append(fullType);
            if (isInherited)
            {
                sb.Append("_ab_");
            }
            else
            {
                sb.Append("_at_");
            }

            sb.Append(astAccessor.Name);
            //sb.Append("_");
            // sb.Append(astAccessor.ReturnType);
            return sb.ToString();
        }

        public virtual string MangleField(AstField astField, bool isInheritedFromBase)
        {
            return MangleField(astField.Name, isInheritedFromBase);
        }

        public virtual string MangleField(string fieldName, bool isInheritedFromBase)
        {
            var sb = new StringBuilder();
            // if inherited from base will contain "b_" else will contain "t_" represents base. this.
            sb.Append(isInheritedFromBase ? "b_" : "t_");
            return sb + MangleName(fieldName);
        }

        public string MangleOpOverload(AstOperatorOverload astOpOverload)
        {
            var sb = new StringBuilder();

            sb.Append("_op_");

            if (astOpOverload.OverloadableOperand == OverloadableOperand.Decrement)
            {
                sb.Append("mm");
            }
            else if (astOpOverload.OverloadableOperand == OverloadableOperand.Division)
            {
                sb.Append("div");
            }
            else if (astOpOverload.OverloadableOperand == OverloadableOperand.Equality)
            {
                sb.Append("ee");
            }
            else if (astOpOverload.OverloadableOperand == OverloadableOperand.False)
            {
                sb.Append("false");
            }
            else if (astOpOverload.OverloadableOperand == OverloadableOperand.GreaterThan)
            {
                sb.Append("gt");
            }
            else if (astOpOverload.OverloadableOperand == OverloadableOperand.GreaterThanEqual)
            {
                sb.Append("gte");
            }
            else if (astOpOverload.OverloadableOperand == OverloadableOperand.Increment)
            {
                sb.Append("pp");
            }
            else if (astOpOverload.OverloadableOperand == OverloadableOperand.LessThan)
            {
                sb.Append("lt");
            }
            else if (astOpOverload.OverloadableOperand == OverloadableOperand.LessThanEqual)
            {
                sb.Append("lte");
            }
            else if (astOpOverload.OverloadableOperand == OverloadableOperand.Minus)
            {
                sb.Append("m");
            }
            else if (astOpOverload.OverloadableOperand == OverloadableOperand.Multiplication)
            {
                sb.Append("multi");
            }
            else if (astOpOverload.OverloadableOperand == OverloadableOperand.Not)
            {
                sb.Append("not");
            }
            else if (astOpOverload.OverloadableOperand == OverloadableOperand.NotEqual)
            {
                sb.Append("ne");
            }
            else if (astOpOverload.OverloadableOperand == OverloadableOperand.Plus)
            {
                sb.Append("p");
            }
            else if (astOpOverload.OverloadableOperand == OverloadableOperand.True)
            {
                sb.Append("true");
            }
            else if (astOpOverload.OverloadableOperand == OverloadableOperand.Minus && astOpOverload.AstParameter2==null )
            {
                sb.Append("um");
            }
            else if (astOpOverload.OverloadableOperand == OverloadableOperand.Plus && astOpOverload.AstParameter2 == null)
            {
                sb.Append("up");
            }
            if (astOpOverload.AstParameter1 != null)
            {
                sb.Append("_");
                sb.Append(astOpOverload.AstParameter1.FullQualifiedType.Length);
                sb.Append(astOpOverload.AstParameter1.FullQualifiedType);
            }
            if (astOpOverload.AstParameter2 != null)
            {
                sb.Append("_");
                sb.Append(astOpOverload.AstParameter2.FullQualifiedType.Length);
                sb.Append(astOpOverload.AstParameter2.FullQualifiedType);
            }
            return sb.ToString();
        }

        public virtual string MangleTypeConvert(AstTypeConverter astTypeConver)
        {
            var sb = new StringBuilder();
            sb.Append("_op_");
            if (astTypeConver is AstImplicitTypeConverter)
            {
                sb.Append("imp_");
            }
            else if (astTypeConver is AstExplicitTypeConverter)
            {
                sb.Append("exp_");
            }
            sb.Append(astTypeConver.ReturnType);
            sb.Append("_");
            sb.Append(astTypeConver.AstParameter.FullQualifiedType);
            return sb.ToString();
        }

        public virtual string MangleTypeConvert(bool isImplicit, string Rettype, string Fqt)
        {
            var sb = new StringBuilder();
            sb.Append("_op_");
            if (isImplicit)
                sb.Append("imp_");
            else
                sb.Append("exp_");
            sb.Append(Rettype);
            sb.Append("_");
            sb.Append(Fqt);
            return sb.ToString();
        }

        public virtual bool IsFieldInBase(string fieldName)
        {
            return fieldName.StartsWith("b_") ? true : false;
        }

        public virtual string DemangleField(string str)
        {
            return DemangleName(str.Substring(2, str.Length - 2));
        }
    }
}