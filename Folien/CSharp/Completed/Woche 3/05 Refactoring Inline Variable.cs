// -*- coding: utf-8 -*-
// %% [markdown]
//
// <div style="text-align:center; font-size:200%;">
//  <b>Refactoring: Inline Variable</b>
// </div>
// <br/>
// <div style="text-align:center; font-size:120%;">Dr. Matthias Hölzl</div>
// <br/>
// <div style="text-align:center;">Coding-Akademie München</div>
// <br/>

// %% [markdown]
//
// ### Inline Variable

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
    var orderLines = new List<OrderLine>
    {
        new OrderLine("Apple", 2, 0.5m),
        new OrderLine("Banana", 3, 0.3m),
        new OrderLine("Orange", 1, 0.8m)
    };
    return orderLines;
}

// %%
public class Refactoring
{
    public static decimal OrderLinePrice(OrderLine orderLine)
    {
        return orderLine.Quantity * orderLine.Price;
    }

    public static decimal ComputeTotal(List<OrderLine> orderLines)
    {
        decimal total = 0.0m;
        foreach (var orderLine in orderLines)
        {
            decimal orderLinePrice1 = OrderLinePrice(orderLine);
            total += orderLinePrice1;
        }
        return total;
    }
}

// %%
Refactoring.ComputeTotal(MakeOrderLines());

// %%
public class Refactoring
{
    public static decimal OrderLinePrice(OrderLine orderLine)
    {
        return orderLine.Quantity * orderLine.Price;
    }

    public static decimal ComputeTotal(List<OrderLine> orderLines)
    {
        decimal total = 0.0m;
        foreach (var orderLine in orderLines)
        {
            total += OrderLinePrice(orderLine); // <-- inline variable
        }
        return total;
    }
}

// %%
Refactoring.ComputeTotal(MakeOrderLines());

// %% [markdown]
//
// #### Motivation
//
// - Manchmal kommuniziert der Name der Variable nicht mehr als der Ausdruck
//   selbst
// - Manchmal verhindert eine Variable das Refactoring von anderem Code

// %% [markdown]
//
// #### Mechanik
//
// - Stelle sicher, dass der Initialisierungs-Ausdruck frei von Seiteneffekten
//   ist
// - Falls die Variable nicht schon konstant ist, mache sie konstant und teste
//   das Programm
// - Finde die erste Referenz auf die Variable
// - Ersetze die Variable durch ihren Initialisierungs-Ausdruck
// - Teste das Programm
// - Wiederhole für jede Referenz auf die Variable
