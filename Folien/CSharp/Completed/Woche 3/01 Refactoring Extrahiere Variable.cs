// -*- coding: utf-8 -*-
// %% [markdown]
//
// <div style="text-align:center; font-size:200%;">
//  <b>Refactoring: Extrahiere Variable</b>
// </div>
// <br/>
// <div style="text-align:center; font-size:120%;">Dr. Matthias Hölzl</div>
// <br/>
// <div style="text-align:center;">Coding-Akademie München</div>
// <br/>

// %% [markdown]
//
// ### Extrahiere Variable

// %%
using System;
using System.Collections.Generic;

// %%
public class OrderLine
{
    public string Product { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }

    public OrderLine(string product, int quantity, decimal price)
    {
        Product = product;
        Quantity = quantity;
        Price = price;
    }
}

// %%
static List<OrderLine> MakeOrderLines()
{
    List<OrderLine> orderLines = new List<OrderLine>();
    orderLines.Add(new OrderLine("Apple", 2, 0.5m));
    orderLines.Add(new OrderLine("Banana", 3, 0.3m));
    orderLines.Add(new OrderLine("Orange", 1, 0.8m));
    return orderLines;
}

// %%
public class Refactoring
{
    public static decimal ComputeTotal(List<OrderLine> orderLines)
    {
        decimal total = 0.0m;
        foreach (OrderLine orderLine in orderLines)
        {
            total += orderLine.Quantity * orderLine.Price;
        }
        return total;
    }
}

// %%
Refactoring.ComputeTotal(MakeOrderLines())

// %%
public class Refactoring
{
    public static decimal ComputeTotal(List<OrderLine> orderLines)
    {
        decimal total = 0.0m;
        foreach (OrderLine orderLine in orderLines)
        {
            decimal orderLinePrice = orderLine.Quantity * orderLine.Price;
            //           ^^^^^^^^^^^^^^ new variable
            total += orderLinePrice;
        }
        return total;
    }
}

// %%
Refactoring.ComputeTotal(MakeOrderLines())

// %% [markdown]
//
// #### Motivation
//
// - Hilft dabei, komplexe Ausdrücke zu verstehen
//   - Erklärende Variablen/Konstanten
// - Debugging oft einfacher

// %% [markdown]
//
// #### Mechanik
//
// - Stelle sicher, dass der Ausdruck frei von Seiteneffekten ist
// - Erzeuge eine neue konstante Variable
// - Initialisiere sie mit dem Ausdruck
// - Ersetze den Ausdruck durch die Variable
// - Teste das Programm
