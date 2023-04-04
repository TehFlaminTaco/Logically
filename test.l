@Main
Bus: 8a, 8i, clock, risingPulse, delayIn, halter;
ByteAdder       (8a, [h,l,l,l,l,l,l,l]) (8i)
ByteCell        (8i, low  )             (8a)
RisingEdge      (H)                     (risingPulse)
OR              (risingPulse, clock)    (delayIn)
Delay           (delayIn)               (clock)
Delay           (high)                  (halter)
WRITE           (clock, [8a])           ()
HALT            (halter)                ()

@2BitAdder
: in0, in1, in2;
: out, carry;
: t0, t1, t2;
XOR  (in0, in1, in2) (out)
AND  (in0, in1)      (t0)
AND  (in1, in2)      (t1)
AND  (in0, in2)      (t2)
OR   (t0, t1, t2)    (carry)

@ByteAdder
: 8a, 8b;
: 8o, carry;
: 7c;
2BitAdder (_0a, _0b, low) (_0o, _0c)
2BitAdder (_1a, _1b, _0c) (_1o, _1c)
2BitAdder (_2a, _2b, _1c) (_2o, _2c)
2BitAdder (_3a, _3b, _2c) (_3o, _3c)
2BitAdder (_4a, _4b, _3c) (_4o, _4c)
2BitAdder (_5a, _5b, _4c) (_5o, _5c)
2BitAdder (_6a, _6b, _5c) (_6o, _6c)
2BitAdder (_7a, _7b, _6c) (_7o, carry)

@RSLatch
: ir, is;
: or, os;
: nr, ns, tr, ts;
NOT  (tr)     (nr)
NOT  (ts)     (ns)
OR   (ir, ns) (tr)
OR   (is, nr) (ts)
COPY (tr, ts) (or, os)

@BitCell
: data, clock;
: out;
: nData, rWrite, sWrite, discard;
NOT (data)               (nData)
AND (data, clock)        (rWrite)
AND (nData, clock)       (sWrite)
RSLatch (rWrite, sWrite) (out)

@ByteCell
: [8i], clock;
: [8o];
BitCell (_0i, clock) (_0o)
BitCell (_1i, clock) (_1o)
BitCell (_2i, clock) (_2o)
BitCell (_3i, clock) (_3o)
BitCell (_4i, clock) (_4o)
BitCell (_5i, clock) (_5o)
BitCell (_6i, clock) (_6o)
BitCell (_7i, clock) (_7o)

@Mux
: i, a, b;
: d;
: ni, ani, bi;
NOT (i)       (ni)
AND (a, ni)   (ani)
AND (b, i)    (bi)
OR  (ani, bi) (d)

@ByteMux
: i, [8a], [8b];
: [8o];
Mux (i, _0a, _0b) (_0o)
Mux (i, _1a, _1b) (_1o)
Mux (i, _2a, _2b) (_2o)
Mux (i, _3a, _3b) (_3o)
Mux (i, _4a, _4b) (_4o)
Mux (i, _5a, _5b) (_5o)
Mux (i, _6a, _6b) (_6o)
Mux (i, _7a, _7b) (_7o)

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
: bar;
NOT (in)      (bar)
AND (in, bar) (pulse)

@Delay
: in;
: out;
: 256d;
COPY (in, 256d) (256d, out)