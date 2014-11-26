/*
 *author: Louise
 *date  : 21/09/2014
 *describe : 记录单词的类型枚举
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Common
{
    /*LexType是终极符*/
    public enum LexType 
    {
        /*簿记单词符号*/
        ENDFILE, ERROR, 
        
        /*保留字*/
        PROGRAM, PROCEDURE, TYPE, VAR, IF,
        THEN, ELSE, FI, WHILE, DO,
        ENDWH, BEGIN, END, READ, WRITE,
        ARRAY, OF, RECORD, RETURN,  
        //类型
        INTEGER, CHAR,

        /*多字符单词符号*/
        ID,          INTC,          CHARC, 

        /*特殊符号*/
        ASSIGN,      EQ,            LT,             PLUS,        MINUS,  
        TIMES,       OVER,          LPAREN,         RPAREN,      DOT,  
        COLON,       SEMI,          COMMA,          LMIDPAREN,   RMIDPAREN,  
        UNDERANGE 
    };

    /*非终极符*/
    public enum nonTerminals
    {
        Program,     ProgramHead,   ProgramName,    DeclarePart, TypeDecpart,  
        TypeDec,     TypeDecList,   TypeDecMore,    TypeId,      TypeDef,  
        BaseType,    StructureType, ArrayType,      Low,         Top,  
        RecType,     FieldDecList,  FieldDecMore,   IdList,      IdMore,  
        VarDecpart,  VarDec,        VarDecList,     VarDecMore,  VarIdList,   VarIdMore,  
        ProcDecpart, ProcDec,       ProcDecMore,    ProcName,    ParamList,   ProcDeclaration ,  
        ParamDecList,ParamMore,     Param,          FormList,    FidMore,  
        ProcDecPart, ProcBody,      ProgramBody,    StmList,     StmMore,     Stm,  
        AssCall,     AssignmentRest,ConditionalStm, LoopStm,     InputStm,  
        Invar,       OutputStm,     ReturnStm,      CallStmRest, ActParamList,  
        ActParamMore,RelExp,        OtherRelE,      Exp,         OtherTerm,  
        Term,        OtherFactor,   Factor,         Variable,    VariMore,    FieldVar,  
        FieldVarMore,CmpOp,         AddOp,          MultOp
    };
    
    public enum nodeKind {  
        ProK ,  PheadK , TypeK , VarK , ProcDecK , StmLK , DecK , StmtK , ExpL 
    };
    public enum decKind {  
        ArrayK , CharK , IntegerK , RecordK , IdK 
    };
    public enum stmtKind {  
        IfK , WhileK , AssignK , ReadK , WriteK , CallK , ReturnK
    };
    public enum expKind {  
        OpK , ConstK , VariK 
    };
    public enum varKind {  
        IdV , ArrayMembV , FieldMembV 
    };
    public enum expType {  
        Void , Integer , Boolean 
    };
    public enum paramType {  
        ValParamType , VarparamType
    };
    public enum TypeKind {
        intTy , charTy , boolTy , arrayTy , fieldTy
    };
    public enum IdKind {
        varkind , typekind , prockind 
    };
    public enum AccessKind{
        dir , indir 
    };
}
