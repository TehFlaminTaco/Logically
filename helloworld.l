@Main
Bus: 8a, 8i, clock, risingPulse, delayIn, 8c;
ByteAdder       (8a, [1,0,0,0,0,0,0,0]) (8i)
CELL            (clock, 8i)             (8a)
RisingEdge      (H)                     (risingPulse)
OR              (risingPulse, clock)    (delayIn)
Delay           (delayIn)               (clock)
WRITE           (clock, [8c])           ()
FByteMux        (4a,
                 0,0,0,1, 0,0,1,0,
                 1,0,1,0, 0,1,1,0,
                 0,0,1,1, 0,1,1,0,
                 0,0,1,1, 0,1,1,0,
                 1,1,1,1, 0,1,1,0,
                 0,0,1,1, 0,1,0,0,
                 0,0,0,0, 0,1,0,0,
                 1,1,1,0, 1,0,1,0,
                 1,1,1,1, 0,1,1,0,
                 0,1,0,0, 1,1,1,0,
                 0,0,1,1, 0,1,1,0,
                 0,0,1,0, 0,1,1,0,
                 1,0,0,0, 0,1,0,0,
                 0,1,0,1, 0,0,0,0,
                 0,0,0,0, 0,0,0,1,
                 0,0,0,0, 0,0,0,0)
                                        (8c)
HALT            (c7)                    ()

@2BitAdder
: 3in;
: out, carry;
: 4t;
XOR  (3in)           (t3)
AND  (in0, in1)      (t0)
AND  (in1, in2)      (t1)
AND  (in0, in2)      (t2)
COPY (t3)            (out)
OR   (3t)            (carry)

@ByteAdder
: 8a, 8b;
: 8o, carry;
: 7c;
2BitAdder (a0, b0, 0)  (o0, c0)
2BitAdder (a1, b1, c0) (o1, c1)
2BitAdder (a2, b2, c1) (o2, c2)
2BitAdder (a3, b3, c2) (o3, c3)
2BitAdder (a4, b4, c3) (o4, c4)
2BitAdder (a5, b5, c4) (o5, c5)
2BitAdder (a6, b6, c5) (o6, c6)
2BitAdder (a7, b7, c6) (o7, carry)

@Mux
: i, a, b;
: d;
: ni, ani, bi;
NOT (i)       (ni)
AND (a, ni)   (ani)
AND (b, i)    (bi)
OR  (ani, bi) (d)

@ByteMux
: i, [8d0], [8d1];
: [8o];
Mux (i, d00, d10) (o0)
Mux (i, d01, d11) (o1)
Mux (i, d02, d12) (o2)
Mux (i, d03, d13) (o3)
Mux (i, d04, d14) (o4)
Mux (i, d05, d15) (o5)
Mux (i, d06, d16) (o6)
Mux (i, d07, d17) (o7)

@4ByteMux
: 1i, i, 8d0, 8d1, 8d2, 8d3;
: 8o;
: 8a, 8b;
ByteMux (1i, 8d0, 8d1) (8a)
ByteMux (1i, 8d2, 8d3) (8b)
ByteMux ( i, 8a, 8b)   (8o)

@8ByteMux
: 2i, i, 8d0, 8d1, 8d2, 8d3, 8d4, 8d5, 8d6, 8d7;
: 8o;
: 8a, 8b;
4ByteMux (2i, 8d0, 8d1, 8d2, 8d3) (8a)
4ByteMux (2i, 8d4, 8d5, 8d6, 8d7) (8b)
ByteMux  ( i, 8a,  8b)            (8o)

@FByteMux
: 3i, i, 8d0, 8d1, 8d2, 8d3, 8d4, 8d5, 8d6, 8d7, 8d8, 8d9, 8dA, 8dB, 8dC, 8dD, 8dE, 8dF;
: 8o;
: 8a, 8b;
8ByteMux (3i, 8d0, 8d1, 8d2, 8d3, 8d4, 8d5, 8d6, 8d7) (8a)
8ByteMux (3i, 8d8, 8d9, 8dA, 8dB, 8dC, 8dD, 8dE, 8dF) (8b)
ByteMux  ( i, 8a, 8b)                                 (8o)

@RisingEdge
: in;
: pulse;
: bar_HIGH;
NOT (in)      (bar)
AND (in, bar) (pulse)

@Delay
: in;
: out;
: 16d;
COPY (in, 16d) (16d, out)