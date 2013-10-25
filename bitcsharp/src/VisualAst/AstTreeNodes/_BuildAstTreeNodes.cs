using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LLVMSharp.Compiler.Ast;
using System.Windows.Forms;

namespace LLVMSharp.VisualAst
{
    public class BuildAstTreeNodes
    {
        TreeView _tv;
        AstProgram _astProgram;

        public BuildAstTreeNodes(TreeView tv, AstProgram astProgram)
        {
            _tv = tv;
            _astProgram = astProgram;
        }

        #region Helpers Methods

        private static TreeNode NewNode(string text, object tag)
        {
            TreeNode node = new TreeNode(text);
            node.Tag = tag;
            return node;
        }

        private static void AddNode(TreeNode parent, TreeNode newNode)
        {
            parent.TreeView.Invoke(new MethodInvoker(() =>
            {
                parent.Nodes.Add(newNode);
            }));
        }

        private static void ExpandNode(TreeNode node)
        {
            node.TreeView.Invoke(new MethodInvoker(() =>
            {
                node.Expand();
            }));
        }

        #endregion

        public void BuildTreeView()
        {
            TreeNode node = NewNode("[Program]", _astProgram);
            _tv.Invoke(new MethodInvoker(() =>
            {
                _tv.Nodes.Add(node);
            }));

            foreach (AstSourceFile item in _astProgram.SourceFiles)
            {
                GenerateAstSourceFilesNodes(item, node);
            }

            ExpandNode(_tv.Nodes[0]);
        }

        private void GenerateAstSourceFilesNodes(AstSourceFile n, TreeNode r)
        {
            TreeNode node = NewNode(
                "[SourceFile] " + n.GetFileNameWithoutExtension(), n);
            AddNode(r, node);

            foreach (AstUsingDeclarative item in n.AstUsingDeclarativeCollection)
                GenerateAstUsingDeclarativeNodes(item, node);

            foreach (AstNamespaceBlock item in n.AstNamespaceBlockCollection)
                GenerateAstNamespaceBlockNodes(item, node);

            foreach (AstType item in n.AstTypeCollection)
                GenerateAstTypeNodes(item, node);

            ExpandNode(node);
        }

        private void GenerateAstTypeNodes(AstType n, TreeNode r)
        {
            string caption = null;
            if (n is AstClass)
                caption = "[class] ";
            else if (n is AstEnum)
                caption = "[enum] ";
            else if (n is AstStruct)
                caption = "[struct] ";

            caption += n.Name;

            TreeNode node = NewNode(caption, n);
            AddNode(r, node);

            foreach (AstTypeModifier item in n.AstTypeModifierCollection)
                GenerateAstModifierNodes(item, node);
            GenerateAstMemberModiferNodes(n.AstMemberModifierCollection, node);

            if (n is AstClass)
            {
                AstClass astClass = (AstClass)n;
                if (astClass.AstParentClass != null)
                    GenerateAstClassParentNode(astClass.AstParentClass, node);
                foreach (AstConstructor item in astClass.AstConstructorCollection)
                    GenerateAstConstructorNodes(item, node);
                GenerateAstFieldCollectionNodes(astClass.AstFieldCollection, node);
                GenerateAstMethodCollectionNodes(astClass.AstMethodCollection, node);
                GenerateAstAccessorCollectionNodes(astClass.AstAccessorCollection, node);
                GenerateAstTypeConverterCollectionNodes(astClass.AstTypeConverterCollection, node);
                GenerateAstOperatorOverloadCollectionNodes(astClass.AstOperatorOverloadCollection, node);
                foreach (AstType item in astClass.AstTypeCollection)
                    GenerateAstTypeNodes(item, node);
            }
            else if (n is AstEnum)
            {
                AstEnum astEnum = (AstEnum)n;
                if (astEnum.AstEnumType != null)
                    GenerateAstEnumTypeNode(astEnum.AstEnumType, node);
                foreach (AstEnumMember item in astEnum.AstEnumMemberCollection)
                    GenerateAstEnumMemberNodes(item, node);
            }
            else if (n is AstStruct)
            {
                AstStruct astStruct = (AstStruct)n;
                foreach (AstConstructor item in astStruct.AstConstructorCollection)
                    GenerateAstConstructorNodes(item, node);
                GenerateAstFieldCollectionNodes(astStruct.AstFieldCollection, node);
                GenerateAstMethodCollectionNodes(astStruct.AstMethodCollection, node);
                GenerateAstAccessorCollectionNodes(astStruct.AstAccessorCollection, node);
                GenerateAstTypeConverterCollectionNodes(astStruct.AstTypeConverterCollection, node);
                GenerateAstOperatorOverloadCollectionNodes(astStruct.AstOperatorOverloadCollection, node);
                foreach (AstType item in astStruct.AstTypeCollection)
                    GenerateAstTypeNodes(item, node);
            }
        }

        private void GenerateAstOperatorOverloadCollectionNodes(AstOperatorOverloadCollection n, TreeNode r)
        {
            foreach (AstOperatorOverload item in n)
            {
                TreeNode node = NewNode("[operator overload] " + AstOperatorOverload.ToString(item.OverloadableOperand),
                    item);
                AddNode(r, node);
                GenerateAstParameterNode(item.AstParameter1, node);
                if (item.AstParameter2 != null)
                    GenerateAstParameterNode(item.AstParameter2, node);

                if (item.AstBlock != null)
                    GenerateAstBlockNode(item.AstBlock, node);
            }
        }

        private void GenerateAstTypeConverterCollectionNodes(AstTypeConverterCollection n, TreeNode r)
        {
            foreach (AstTypeConverter item in n)
            {
                TreeNode node = null;
                if (item is AstImplicitTypeConverter)
                {
                    node = NewNode("[implicit operator] ", item);
                }
                else // AstExplicitTypeConverter
                {
                    node = NewNode("[explicit operator]", item);
                }
                AddNode(r, node);
                GenerateAstParameterNode(item.AstParameter, node);

                if (item.AstBlock != null)
                    GenerateAstBlockNode(item.AstBlock, node);
            }
        }

        private void GenerateAstAccessorCollectionNodes(AstAccessorCollection n, TreeNode r)
        {
            foreach (AstAccessor item in n)
            {
                TreeNode node = NewNode("[accessor] " + item.Name, item);
                AddNode(r, node);
                if (item.AstGetAccessor != null)
                    GenerateAstAccessorNode(item.AstGetAccessor, node);
                if (item.AstSetAccessor != null)
                    GenerateAstAccessorNode(item.AstSetAccessor, node);
            }
        }

        private void GenerateAstAccessorNode(AstGetAccessor n, TreeNode r)
        {
            TreeNode node = NewNode("[get]", n);
            AddNode(r, node);

            if (n.AstBlock != null)
                GenerateAstBlockNode(n.AstBlock, node);
        }

        private void GenerateAstAccessorNode(AstSetAccessor n, TreeNode r)
        {
            TreeNode node = NewNode("[set]", n);
            AddNode(r, node);

            if (n.AstBlock != null)
                GenerateAstBlockNode(n.AstBlock, node);
        }

        private void GenerateAstMethodCollectionNodes(AstMethodCollection n, TreeNode r)
        {
            foreach (AstMethod item in n)
            {
                TreeNode node = NewNode("[method] " + item.Name, item);
                AddNode(r, node);

                foreach (AstParameter param in item.Parameters)
                    GenerateAstParameterNode(param, node);

                if (item.AstBlock != null)
                    GenerateAstBlockNode(item.AstBlock, node);
            }
        }
        private void GenerateAstFunctionCallNodes(AstMethod n, TreeNode r)
        {
            TreeNode node = NewNode("Function Call " + n.Name, r);
            AddNode(r, node);
            //GenerateAstArgumentCollectionNodes(n.ArgumentCollection, node);
        }
        private void GenerateAstFieldCollectionNodes(AstFieldCollection n, TreeNode r)
        {
            foreach (AstField item in n)
            {
                if (!item.IsConstant)
                    GenerateAstFieldNode(item, r);
                else
                    GenerateConstantAstFieldNode(item, r);
            }
        }

        private void GenerateAstFieldNode(AstField n, TreeNode r)
        {
            TreeNode node = NewNode("[field] " + n.Type + " " + n.Name, n);
            AddNode(r, node);
            GenerateAstMemberModiferNodes(n.AstMemberModifierCollection, node);
            if (n.Initialization != null)
                GenerateInitializationNodes(n.Initialization, node);

        }

        private void GenerateInitializationNodes(IAstExpression n, TreeNode r)
        {
            TreeNode node = new TreeNode("[init]");
            AddNode(r, node);
            GenerateExpressionNodes(n, node);
        }

        private void GenerateConstantAstFieldNode(AstField n, TreeNode r)
        {
            TreeNode node = NewNode("[field - const] " + n.Type + " " + n.Name, n);
            AddNode(r, node);
            GenerateAstMemberModiferNodes(n.AstMemberModifierCollection, node);
            if (n.Initialization != null)
                GenerateInitializationNodes(n.Initialization, node);
        }

        private void GenerateExpressionNodes(IAstExpression n, TreeNode r)
        {
            if (n is AstVariableReference)
                GenerateAstVariableReferenceNode((AstVariableReference)n, r);
            else if (n is AstConstant)
                GenerateAstConstantNode((AstConstant)n, r);
            else if (n is AstNull)
                GenerateAstNullNode((AstNull)n, r);
            else if (n is AstAssignmentExpression)
                GenerateAstAssignmentExpressionNodes((AstAssignmentExpression)n, r);
            else if (n is AstBinaryExpression)
                GenerateAstBinaryExpressionNodes((AstBinaryExpression)n, r);
            else if (n is AstIsExpression)
                GenerateAstIsExpressionNode((AstIsExpression)n, r);
            else if (n is AstAsExpression)
                GenerateAstAsExpression((AstAsExpression)n, r);
            else if (n is AstTypeOf)
                GenerateAstTypeOf((AstTypeOf)n, r);
            else if (n is AstSizeOf)
                GenerateAstSizeOf((AstSizeOf)n, r);
            else if (n is AstPostIncrement)
                GenerateAstPostIncrement((AstPostIncrement)n, r);
            else if (n is AstPostDecrement)
                GenerateAstPostDecrement((AstPostDecrement)n, r);
            else if (n is AstPreDecrement)
                GenerateAstPreDecrement((AstPreDecrement)n, r);
            else if (n is AstPreIncrement)
                GenerateAstPreIncrementNodes((AstPreIncrement)n, r);
            else if (n is AstNot)
                GenerateAstNotNodes((AstNot)n, r);
            else if (n is AstNewType)
                GenerateAstNewTypeNodes((AstNewType)n, r);
            else if (n is AstMemberReference)
                GenerateAstMemberReferenceNode((AstMemberReference)n, r);
            else if (n is AstUnaryMinus)
                GenerateAstUnaryMinusNodes((AstUnaryMinus)n, r);
            else if (n is AstUnaryPlus)
                GenerateAstUnaryPlusNodes((AstUnaryPlus)n, r);
            else if (n is AstMethodCall)
                GenerateAstMethodCallNodes((AstMethodCall)n, r);
            else if (n is AstTypeCast)
                GenerateAstTypeCastNodes((AstTypeCast)n, r);
            else if (n is AstArrayInitialization)
                GenerateAstArrayInitializationNodes((AstArrayInitialization)n, r);
            else if (n is AstIndexer)
                GenerateAstIndexerNode((AstIndexer)n, r);
        }

        private void GenerateAstMethodCallNodes(AstMethodCall n, TreeNode r)
        {
            TreeNode node = NewNode("[method call " + n.Name + "]", n);
            AddNode(r, node);
            if (n.ArgumentCollection != null)
            {
                GenerateAstArgumentCollectionNodes(n.ArgumentCollection, node);
            }
        }
        private void GenerateAstTypeCastNodes(AstTypeCast n, TreeNode r)
        {
            TreeNode node = NewNode("[Type Cast] " + n.Type, n);
            AddNode(r, node);
            GenerateExpressionNodes(n.AstExpression, node);
        }
        private void GenerateAstNewTypeNodes(AstNewType n, TreeNode r)
        {
            TreeNode node = NewNode("[new " + n.Type + "]", n);
            AddNode(r, node);

            if (n.AstArgumentCollection != null)
                GenerateAstArgumentCollectionNodes(n.AstArgumentCollection, node);
            else
            {
                GenerateAstIndexerNode(n.AstIndexer, node);
                GenerateAstArrayInitializationNodes(n.AstArrayInitialization, node);
            }
        }

        private void GenerateAstIndexerNode(AstIndexer n, TreeNode r)
        {
            if (n == null)
                return;
            TreeNode node = NewNode("[array-indexer]", n);
            AddNode(r, node);
            GenerateExpressionNodes(n.Expression, node);
        }

        private void GenerateAstArrayInitializationNodes(AstArrayInitialization n, TreeNode r)
        {
            if (n == null)
                return;
            TreeNode node = NewNode("[array-init]", n);
            AddNode(r, node);
            foreach (IAstExpression item in n.AstExpressionCollection)
                GenerateExpressionNodes(item, node);
        }
        private void GenerateAstUnaryMinusNodes(AstUnaryMinus n, TreeNode r)
        {
            TreeNode node = NewNode("[Unary Minus]", n);
            AddNode(r, node);
            GenerateExpressionNodes(n.AstExpression, node);
        }
        private void GenerateAstUnaryPlusNodes(AstUnaryPlus n, TreeNode r)
        {
            TreeNode node = NewNode("[Unary Plus]", n);
            AddNode(r, node);
            GenerateExpressionNodes(n.AstExpression, node);
        }
        private void GenerateAstNotNodes(AstNot n, TreeNode r)
        {
            TreeNode node = NewNode("[Not !]", n);
            AddNode(r, node);
            GenerateExpressionNodes(n.AstExpression, node);
        }

        private void GenerateAstPreIncrementNodes(AstPreIncrement n, TreeNode r)
        {
            TreeNode node = NewNode("[PreIncrement]", n);
            AddNode(r, node);
            GenerateExpressionNodes(n.AstExpression, node);
        }

        private void GenerateAstPreDecrement(AstPreDecrement n, TreeNode r)
        {
            TreeNode node = NewNode("[PreDecrement]", n);
            AddNode(r, node);
            GenerateExpressionNodes(n.AstExpression, node);
        }

        private void GenerateAstPostDecrement(AstPostDecrement n, TreeNode r)
        {
            TreeNode node = NewNode("[PostDecrement]", n);
            AddNode(r, node);
            GenerateExpressionNodes(n.AstExpression, node);
        }

        private void GenerateAstPostIncrement(AstPostIncrement n, TreeNode r)
        {
            TreeNode node = NewNode("[PostIncrement]", n);
            AddNode(r, node);
            GenerateExpressionNodes(n.AstExpression, node);
        }

        private void GenerateAstSizeOf(AstSizeOf n, TreeNode r)
        {
            TreeNode node = NewNode("[sizeof]", n);
            AddNode(r, node);
        }

        private void GenerateAstTypeOf(AstTypeOf n, TreeNode r)
        {
            TreeNode node = NewNode("[typeof]", n);
            AddNode(r, node);
        }

        private void GenerateAstAssignmentExpressionNodes(AstAssignmentExpression n, TreeNode r)
        {
            string caption = "[Assignment Expression ";
            if (n is AstSimpleAssignmentExpression)
                caption += "= ]";
            else if (n is AstAddAssignmentExpression)
                caption += "+= ]";
            else if (n is AstSubtractAssignmentExpression)
                caption += "-= ]";
            else if (n is AstMultiplyAssignmentExpression)
                caption += "*= ]";
            else if (n is AstDivisionAssignmentExpression)
                caption += "/= ]";
            TreeNode node = NewNode(caption, n);
            AddNode(r, node);
            GenerateExpressionNodes(n.LValue, node);
            GenerateExpressionNodes(n.RValue, node);
        }

        private void GenerateAstAsExpression(AstAsExpression n, TreeNode r)
        {
            TreeNode node = NewNode("[as operator]", n);
            AddNode(r, node);
            AddNode(node, new TreeNode("type - " + n.Type));
            GenerateExpressionNodes(n.AstExpression, node);
        }

        private void GenerateAstIsExpressionNode(AstIsExpression n, TreeNode r)
        {
            TreeNode node = NewNode("[is operator]", n);
            AddNode(r, node);
            AddNode(node, new TreeNode("type - " + n.Type));
            GenerateExpressionNodes(n.AstExpression, node);
        }

        private void GenerateAstBinaryExpressionNodes(AstBinaryExpression n, TreeNode r)
        {
            string caption = "[binary expr - ";
            if (n is AstMuliplicationExpression)
                caption += "mul * ]";
            else if (n is AstDivisionExpression)
                caption += "div / ]";
            else if (n is AstAdditionExpression)
                caption += "add + ]";
            else if (n is AstSubtractionExpression)
                caption += "sub - ]";
            else if (n is AstLesserThanExpression)
                caption += "condition < ]";
            else if (n is AstLesserThanOrEqualExpression)
                caption += "condition <= ]";
            else if (n is AstGreaterThanExpression)
                caption += "condition > ]";
            else if (n is AstGreaterThanOrEqualExpression)
                caption += "condition >= ]";
            else if (n is AstEqualityExpression)
                caption += "condtion == ]";
            else if (n is AstInequalityExpression)
                caption += "condition != ]";
            else if (n is AstAndExpression)
                caption += "relational && ]";
            else if (n is AstOrExpression)
                caption += "relational || ]";

            TreeNode node = NewNode(caption, n);
            AddNode(r, node);
            GenerateExpressionNodes(n.LeftOperand, node);
            GenerateExpressionNodes(n.RightOperand, node);
        }

        private void GenerateAstNullNode(AstNull n, TreeNode r)
        {
            TreeNode node = NewNode("[null]", n);
            AddNode(r, node);
        }

        private void GenerateAstConstantNode(AstConstant n, TreeNode r)
        {
            string caption = "";
            if (n is AstIntegerConstant)
                caption = "[int constant]";
            else if (n is AstRealConstant)
                caption = "[real constant]";
            else if (n is AstStringConstant)
                caption = "[string constant]";
            else if (n is AstBooleanConstant)
                caption = "[bool constant]";
            else if (n is AstCharConstant)
                caption += "[char constant]";
            TreeNode node = NewNode(caption, n);
            AddNode(r, node);
        }

        private void GenerateAstVariableReferenceNode(AstVariableReference n, TreeNode r)
        {

            TreeNode node = NewNode("[VarRef] " + n.VariableName, n);
            AddNode(r, node);
            if (n.Indexer != null)
            {
                TreeNode node2 = NewNode("Indexer", n);
                AddNode(node, node2);
                GenerateExpressionNodes(n.Indexer, node2);
            }
        }
        private void GenerateAstMemberReferenceNode(AstMemberReference n, TreeNode r)
        {
            TreeNode node = NewNode(" [VarRef] " + n.Type, n);
            AddNode(r, node);
        }
        private void GenerateAstLiteralVarReferenceNode(AstLiteralVarReference n, TreeNode r)
        {
            TreeNode node = NewNode(" [Literal VarRef] " + n.VariableName,n);
            AddNode(r, node);
        }
        private void GenerateAstConstructorNodes(AstConstructor n, TreeNode r)
        {
            TreeNode node = NewNode("[ctor]", n);
            AddNode(r, node);

            foreach (AstParameter item in n.Parameters)
                GenerateAstParameterNode(item, node);

            if (n.AstConstructorCall != null)
                GenerateAstConstructorCallNodes(n.AstConstructorCall, node);

            if (n.AstBlock != null)
                GenerateAstBlockNode(n.AstBlock, node);
        }

        private void GenerateAstConstructorCallNodes(AstConstructorCall n, TreeNode r)
        {
            string caption = n is AstThisConstructorCall ? "[ :this() ]" : "[ :base() ]";

            TreeNode node = NewNode(caption, n);
            AddNode(r, node);

            GenerateAstArgumentCollectionNodes(n.AstArgumentCollection, node);
        }

        private void GenerateAstArgumentCollectionNodes(AstArgumentCollection n, TreeNode r)
        {
            if (n != null)
            {
                foreach (AstArgument item in n)
                {
                    TreeNode node = NewNode("[argument]", item);
                    AddNode(r, node);
                    GenerateExpressionNodes(item.AstExpression, node);
                }
            }
        }

        private void GenerateAstBlockNode(AstBlock n, TreeNode r)
        {
            TreeNode node = NewNode("[block]", n);
            AddNode(r, node);

            foreach (AstStatement item in n.AstStatementCollection)
                GenerateAstStatementNodes(item, node);
        }

        private void GenerateAstStatementNodes(AstStatement n, TreeNode r)
        {

            if (n is AstLocalVariableDeclaration)
                GenerateAstLocalVariableDeclaration((AstLocalVariableDeclaration)n, r);
            else if (n is AstBlock)
                GenerateAstBlockNode((AstBlock)n, r);
            else if (n is AstIfCondition)
                GenerateAstIfConditionNode((AstIfCondition)n, r);
            else if (n is AstForLoop)
                GenerateAstForLoopNode((AstForLoop)n, r);
            else if (n is AstWhileLoop)
                GenerateAstWhileLoopNode((AstWhileLoop)n, r);
            else if (n is AstDoLoop)
                GenerateAstDoLoopNode((AstDoLoop)n, r);
            else if (n is AstBreak)
                GenerateAstBreakNode((AstBreak)n, r);
            else if (n is AstContinue)
                GenerateAstContinueNode((AstContinue)n, r);
            else if (n is AstReturn)
                GenerateAstReturnNode((AstReturn)n, r);
            else if (n is AstAssignmentStatement)
                GenerateAstAssignmentStatementNodes((AstAssignmentStatement)n, r);
            else if (n is AstPreIncrement)
                GenerateAstPreIncrementNodes((AstPreIncrement)n, r);
            else if (n is AstPostIncrement)
                GenerateAstPostIncrement((AstPostIncrement)n, r);
            else if (n is AstPreDecrement)
                GenerateAstPreDecrement((AstPreDecrement)n, r);
            else if (n is AstPostDecrement)
                GenerateAstPostDecrement((AstPostDecrement)n, r);
            else if (n is AstMethodCall)
                GenerateAstMethodCallNodes((AstMethodCall)n, r);
        }

        private void GenerateAstForLoopNode(AstForLoop n, TreeNode r)
        {
            TreeNode node = NewNode("[for loop]", n);
            AddNode(r, node);

            TreeNode initNode = new TreeNode("[init]");
            AddNode(node, initNode);

            foreach (AstStatement item in n.Initializers)
                GenerateAstStatementNodes(item, initNode);

            TreeNode conditionNode = new TreeNode("[condition]");
            AddNode(node, conditionNode);
            GenerateExpressionNodes(n.Condition, conditionNode);

            TreeNode incrementNode = new TreeNode("[increment]");
            AddNode(node, incrementNode);
            foreach (IAstExpression item in n.IncrementExpressions)
                GenerateExpressionNodes(item, incrementNode);

            TreeNode bodyNode = new TreeNode("[body]");
            AddNode(node, bodyNode);
            GenerateAstStatementNodes(n.Body, bodyNode);
        }

        private void GenerateAstAssignmentStatementNodes(AstAssignmentStatement n, TreeNode r)
        {
            TreeNode node = NewNode("[assignment stmt]", n);
            AddNode(r, node);
            if (n.AstAssignmentExpression != null)
                GenerateAstAssignmentExpressionNodes(n.AstAssignmentExpression, node);
        }

        private void GenerateAstReturnNode(AstReturn n, TreeNode r)
        {
            TreeNode node = NewNode("[return]", n);
            AddNode(r, node);
            if (n.AstExpression != null)
                GenerateExpressionNodes(n.AstExpression, node);
        }

        private void GenerateAstContinueNode(AstContinue n, TreeNode r)
        {
            TreeNode node = NewNode("[continue]", n);
            AddNode(r, node);
        }

        private void GenerateAstBreakNode(AstBreak n, TreeNode r)
        {
            TreeNode node = NewNode("[break]", n);
            AddNode(r, node);
        }

        private void GenerateAstDoLoopNode(AstDoLoop n, TreeNode r)
        {
            TreeNode node = NewNode("[do]", n);
            AddNode(r, node);

            GenerateConditionNodes(n.Condition, node);

            if (n.AstStatement != null)
                GenerateAstStatementNodes(n.AstStatement, node);
        }

        private void GenerateAstWhileLoopNode(AstWhileLoop n, TreeNode r)
        {
            TreeNode node = NewNode("[while]", n);
            AddNode(r, node);

            GenerateConditionNodes(n.Condition, node);

            if (n.AstStatement != null)
                GenerateAstStatementNodes(n.AstStatement, node);
        }

        private void GenerateAstIfConditionNode(AstIfCondition n, TreeNode r)
        {
            TreeNode node = NewNode("[if]", n);
            AddNode(r, node);

            GenerateConditionNodes(n.Condition, node);

            GenerateAstStatementNodes(n.AstStatement, node);

            if (n.AstStatementElse != null)
            {
                TreeNode elseNode = NewNode("[else]", null);
                AddNode(node, elseNode);
                GenerateAstStatementNodes(n.AstStatementElse, elseNode);
            }
        }
        private void GenerateConditionNodes(IAstExpression n, TreeNode r)
        {
            TreeNode node = new TreeNode("[condition]");
            AddNode(r, node);
            GenerateExpressionNodes(n, node);
        }

        private void GenerateAstLocalVariableDeclaration(AstLocalVariableDeclaration n, TreeNode r)
        {
            string caption = "[LocalVar";
            if (n.IsConstant)
                caption += " - const";
            TreeNode node = NewNode(caption + "] " + n.Type + " " + n.Name, n);
            AddNode(r, node);
            if (n.Initialization != null)
                GenerateInitializationNodes(n.Initialization, node);
        }

        private void GenerateAstParameterNode(AstParameter n, TreeNode r)
        {
            TreeNode node = null;
            if (n.ParameterType is AstPassByValueParameterType)
                node = NewNode("[param] " + n.Type + " " + n.Name, n);
            else
                node = NewNode("[param - ref] " + n.Type + " " + n.Name, n);
            AddNode(r, node);
        }

        private void GenerateAstMemberModiferNodes(AstMemberModifierCollection n, TreeNode r)
        {
            foreach (AstMemberModifier item in n)
            {
                string caption = "[member modifier] ";
                if (item is AstPublicMemberModifier)
                    caption += "public";
                else if (item is AstPrivateMemberModifier)
                    caption += "private";
                else if (item is AstProtectedMemberModifier)
                    caption += "protected";
                else if (item is AstSealedMemberModifier)
                    caption += "sealed";
                else if (item is AstStaticMemberModifier)
                    caption += "static";
                else if (item is AstOverrideMemberModifier)
                    caption += "override";
                else if (item is AstVirtualMemberModifier)
                    caption += "virtual";
                else if (item is AstExternMemberModifier)
                    caption += "extern";

                TreeNode node = NewNode(caption, item);
                AddNode(r, node);
            }
        }

        private void GenerateAstEnumMemberNodes(AstEnumMember n, TreeNode r)
        {
            TreeNode node = NewNode("[enum member] " + n.Name, n);
            AddNode(r, node);
        }

        private void GenerateAstEnumTypeNode(AstEnumType n, TreeNode r)
        {
            TreeNode node = NewNode("[enum type] " + n.Name, n);
            AddNode(r, node);
        }

        private void GenerateAstClassParentNode(AstParentClass n, TreeNode r)
        {
            TreeNode node = NewNode("[Parent Class] " + n.Name, n);
            AddNode(r, node);
        }
        private void GenerateAstModifierNodes(AstTypeModifier n, TreeNode r)
        {
            string caption = "[type modifier] ";
            if (n is AstPublicTypeModifier)
                caption += "public";
            else if (n is AstPrivateTypeModifier)
                caption += "private";
            else if (n is AstProtectedTypeModifier)
                caption += "protected";
            else if (n is AstSealedTypeModifier)
                caption += "sealed";

            TreeNode node = NewNode(caption, n);
            AddNode(r, node);
        }

        private void GenerateAstNamespaceBlockNodes(AstNamespaceBlock n, TreeNode r)
        {
            TreeNode node = NewNode("[ns block] " + n.Namespace, n);
            AddNode(r, node);

            foreach (AstNamespaceBlock item in n.AstNamespaceBlockCollection)
                GenerateAstNamespaceBlockNodes(item, node);
            foreach (AstType item in n.AstTypeCollection)
                GenerateAstTypeNodes(item, node);

            ExpandNode(node);
        }

        private void GenerateAstUsingDeclarativeNodes(AstUsingDeclarative n, TreeNode r)
        {
            TreeNode node = NewNode(
                "[UsingDecl] " + n.Namespace, n);
            AddNode(r, node);
        }
    }
}
