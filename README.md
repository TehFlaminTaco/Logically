# Logically
Logically is an Esoteric Langauge based on Logic Gates.

Programs consist of several chips, one of which acts as a "Main" chip.

Unless otherwise specified, all numbers are in Little Endian format.

## Chip Definition
```
@RisingEdge
Inp: in;
Out: pulse;
Bus: bar_HIGH;
NOT (in)      (bar)
AND (in, bar) (pulse)
```
### Name
Every chip begins with `@` followed by the chip name.

### Groups
Then, up to 3 groups of Wires are defined.
`Input:`
`Output:`
`Bus:`

Wire Groups consist of an optional name, followed by a `:`, an then a list of [Wire](#Wire)s.

If the name begins with an `i`, `o`, or `b`, the group will be assumed to be of `Input`, `Output` or `Bus` respectively. Otherwise, they will be defined in order.

* `Input` wires are automatically set to the inputs passed from Connetions
* `Output` wires pass back to the Connection
* `Bus` wires are purely internal.

Every wire group may be terminated with a `;`, for the final group, this is not optional. This is also not optional if wire group names are used.

Non-alphanumeric characters in a wire group are safely ignored.

An extreme example of a set of group definitions are:
```
: [a, b, c] d;
b: e
: f;
```
This defines 3 input wires `a`, `b`, `c` and `d`. One bus wire, `e`. And one output wire, `f`.

### Connections
After the groups are defined, any number of Connections may be defined. A connection is defined as a chip name, followed by a group of input wires, then a group of output wires.

Both the Input and Output wire groups must be surrounded in Parentheses, though like a wire group, other non-alphanumeric characters are ignored.

Each connection represents a distinct instance of a chip.


## Wire
Wires will hold their state as either `low` or `high` (low by default) untill acted upon by either a connection, or if they are an input wire. Input wires will update their state every [tick](#tick).

A wire name consists of an amount of Alphanumeric `[a-zA-Z9-9_]` characters.

Wires which start with a number are a macro which unroll to a number of wires. Eg. `8a` becomes `a0 a1 a2 a3 a4 a5 a6 a7`.

If a wire definition ends in `_HIGH`, it will be removed and the wire's state will default to on.

In connection definitions, wires can be replaced with one of `0`, `low`, `l` or `1`, `high`, `h` to use a constant value for an input. In outputs, `_` can be used to discard the result.

## Tick
A Tick represents one simulation step.

A Tick is processed in the following steps, Starting at the Main Chip
 * The Chips Inputs are set to the received inputs
 * Each Connection in no garunteed order is Ticked
 * The output of each Connection is written to their target wires (In no garunteed order)

Importantly, the results of a Connection isn't usable until the following tick.

## Built-In Chips

* `NOT (...) (...)`: The Inverse of all inputs
* `OR (...) (out)`: Returns HIGH if any input is HIGH, LOW otherwise.
* `AND (...) (out)`: Returns HIGH if all inputs are HIGH, LOW otherwise.
* `XOR (...) (out)`: Returns HIGH if an odd number of inputs are HIGH, LOW otherwise.
* `HALT (clock, ...) (...)`: Stops ticking and returns all inputs when clock is HIGH. Does nothing otherwise.
* `READ (clock) (eof, 8b)`: When `clock` is raised HIGH, sets outputs to the bits of the next byte in STDIN and eof to LOW. If nothing is left in STDIN, EOF is set to high and the other bits aren't written.
* `WRITE (clock, 8b) ()`: When `clock` is raised HIGH, writes the byte represented by the remaining inputs to STDOUT.
* `COPY (...) (...)`: Returns all the inputs.
* `CELL (clock, ...) (...)`: Returns the inputs as they where when `clock` was last raised HIGH.
* `RAND () (64b)`: Randomly set all outputs either HIGH or LOW.

## Command-Line Interface

Programs are executed on the command line in the form of `logically [file] (inputs...)`. Flags may additionally be specified each starting with a `/`.

### Flags
 * `/i`: Input mode. One of:
   * `/ih`: Input parsed as Little-Endian Hexadecimal Digits.
   * `/ib`: Input parsed as Decimal numbers and passed as Little-Endian bytes.
   * Otherwise, Input parsed as any 1's, 0's, h's or l's representing states.
 * `/o`: Output mode. One of:
   * `/oh`: Output as Little-Endian Hexadecimal Digits.
   * `/ob`: Output as Decimal numbers from Little-Endian bytes.
   * `/oq`: No Output
   * Otherwise, Output as 0s and 1s representing states.
 * `/d`: Debug. Dumps the Main Chip's definition and final state into STDERR.