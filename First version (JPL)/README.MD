
# JPL

## *OBS.: comecei este projeto em meados de 2014, trabalhei nele até meados de

2015, desde então não pude voltar a trabalhar nele (foquei em alguns outros projetos),
mesmo trabalhando nele por muito tempo, acredito que está 40% completo. Creio que
sua base já está pronta, teria de reler todo o código para produzir sua documentação,
mas o programei de forma que cada passo do compilador seja reportado, facilitando
assim sua compreensão. Também terei de implementar do zero o assember, pois não
estou achando o que já tinha feito em meus projetos.

## Projeto: criação de linguagem de alto nível orientada a objetos, apelidada de JPL.

O projeto divide-se em duas partes: **o compilador (JPL)** e o **assembler (PAsm)** ;

A **JPL** será compilada em **PAsm** , que será assemblado em código de máquina nativo
(inicialmente **x86** e **x64** ) para **Windows.**

A sintaxe da **JPL** será majoritariamente semelhante à de **C#,** linguagem a qual será
implementado o primeiro compilador.

A sintaxe do **PAsm** será inovadora, um assembly de quase alto nível, de fácil
entendimento.

- **Anexarei um print de como era a sintaxe do assembler que já tinha**
    **implementado.**

## Status: boa parte da base do compilador já foi implementada.

A **análise léxica** e **sintática** já está com sua **base** pronta; a **análise semântica** começou
a ser implementada, acredito que sua base já esteja quase pronta.

## Rotina de compilação:

**Scanner** – analisador léxico: realiza a análise léxica e retorna uma lista com os Tokens.

**ResolveDirectives:** resolve às diretivas e retorna como resultado uma lista de Tokens a
ser passado para o Parser (Análise sintática).

**PreProcess:** responsável pela terceira etapa da compilação, analisa os tokens
resultantes da análise léxica e da procura de diretivas, com objetivo de “reconhecer” o
código, evitando assim que as classes e métodos públicos e estáticos devam ser
primeiro declarados e depois assinalados.

**Parser** – analisador sintático: realiza a análise sintática do código.

**Checker** – analisador semântico: realiza a análise semântica do código.

- **Deixarei anexado também arquivos de texto com a estrutura básica de**
    **algumas rotinas do compilador.**
	
*******************************************************************************************************


# ENGLISH

## * NOTE: I started this project in mid 2014, I worked on it until mid

2015, since then I haven't been able to work on it again (I focused on some other projects),
even working on it for a long time, I believe it is 40% complete. I believe that
your base is already ready, you would have to reread all the code to produce your documentation,
but I programmed it so that each step of the compiler is reported, making it easier
so your understanding. I will also have to implement assember from scratch, because
I am finding what I had already done in my projects.

## Project: creation of a high-level object-oriented language, nicknamed JPL.


The project is divided into two parts: the compiler (JPL) and assembler (PAsm);

JPL will be compiled into PAsm, which will be assembled in native machine code
(initially x86 and x64) for Windows.

The JPL syntax will be mostly similar to C #, language which will be
implemented the first compiler.

The PAsm syntax will be innovative, an almost high-level assembly, easy to
understanding.

- I will attach a print of what was the syntax of the assembler I already had
    implemented.

## Status: much of the compiler base has already been implemented.

The lexical analysis and syntax is already with its base ready; semantic analysis started
to be implemented, I believe that its base is almost ready.

## Build routine:

- Scanner - lexical analyzer: performs the lexical analysis and returns a list of Tokens.

- ResolveDirectives: resolves to the directives and returns as a result a list of Tokens to
be passed to Parser (parsing).

- PreProcess: responsible for the third stage of the compilation, analyzes the tokens
resulting from the lexical analysis and the search for directives, in order to “recognize” the
thus avoiding that public and static classes and methods should be
first declared and then marked.

- Parser: performs the parsing of the code.

- Checker: semantic analyzer: performs the semantic analysis of the code.