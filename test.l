@Main
Bus: [a0,a1,a2,a3,a4,a5,a6,a7], [i0,i1,i2,i3,i4,i5,i6,i7], clock, risingPulse, delayIn;
ByteAdder ([a0,a1,a2,a3,a4,a5,a6,a7], [h,l,l,l,l,l,l,l]) (i0,i1,i2,i3,i4,i5,i6,i7)
ByteCell  ([i0,i1,i2,i3,i4,i5,i6,i7], clock) (a0,a1,a2,a3,a4,a5,a6,a7)
RisingEdge (H) (risingPulse)
OR (risingPulse, clock) (delayIn)
Delay (delayIn) (clock)
WRITE (clock, [a0,a1,a2,a3,a4,a5,a6,a7]) ()
HALT (a7) ()

@2BitAdder
: in0, in1, carryIn;
: out, carry;
: t0;
XOR  (in0, in1, carryIn) (out)
AND  (in0, in1)          (t0)
OR   (t0,  carryIn)      (carry)

@ByteAdder
: [a0,a1,a2,a3,a4,a5,a6,a7], [b0,b1,b2,b3,b4,b5,b6,b6,b7];
: [o0,o1,o2,o3,o4,o5,o6,o7],  carry;
: c0,c1,c2,c3,c4,c5,c6;
2BitAdder (a0, b0)     (o0, c0)
2BitAdder (a1, b1, c0) (o1, c1)
2BitAdder (a2, b2, c1) (o2, c2)
2BitAdder (a3, b3, c2) (o3, c3)
2BitAdder (a4, b4, c3) (o4, c4)
2BitAdder (a5, b5, c4) (o5, c5)
2BitAdder (a6, b6, c5) (o6, c6)
2BitAdder (a7, b7, c6) (o7, carry)

@RSLatch
: ir, is;
: or, os;
: nr, ns;
NOT (or)     (nr)
NOT (os)     (ns)
OR  (ir, ns) (or)
OR  (is, nr) (os)

@BitCell
: data, clock;
: out;
: nData, rWrite, sWrite;
NOT (data)         (nData)
AND (data, clock)        (rWrite)
AND (nData, clock)       (sWrite)
RSLatch (rWrite, sWrite) (out)

@ByteCell
: [i0,i1,i2,i3,i4,i5,i6,i7], clock;
: [o0,o1,o2,o3,o4,o5,o6,o7];
BitCell (i0, clock) (o0)
BitCell (i1, clock) (o1)
BitCell (i2, clock) (o2)
BitCell (i3, clock) (o3)
BitCell (i4, clock) (o4)
BitCell (i5, clock) (o5)
BitCell (i6, clock) (o6)
BitCell (i7, clock) (o7)

@Mux
: i, a, b;
: d;
: ni, ani, bi;
NOT (i)       (ni)
AND (a, ni)   (ani)
AND (b, i)    (bi)
OR  (ani, bi) (d)

@ByteMux
: i, [a0,a1,a2,a3,a4,a5,a6,a7], [b0,b1,b2,b3,b4,b5,b6,b7];
: [o0,o1,o2,o3,o4,o5,o6,o7];
Mux (i, a0, b0) (o0)
Mux (i, a1, b1) (o1)
Mux (i, a2, b2) (o2)
Mux (i, a3, b3) (o3)
Mux (i, a4, b4) (o4)
Mux (i, a5, b5) (o5)
Mux (i, a6, b6) (o6)
Mux (i, a7, b7) (o7)

@4ByteMux
: [i0,i1], [d00,d01,d02,d03,d04,d05,d06,d07],
           [d10,d11,d12,d13,d14,d15,d16,d17],
           [d20,d21,d22,d23,d24,d25,d26,d27],
           [d30,d31,d32,d33,d34,d35,d36,d37];
: [o0,o1,o2,o3,o4,o5,o6,o7];
: [a0,a1,a2,a3,a4,a5,a6,a7], [b0,b1,b2,b3,b4,b5,b6,b7];
ByteMux (i0, [d00,d01,d02,d03,d04,d05,d06,d07],
             [d10,d11,d12,d13,d14,d15,d16,d17])
        (a0,a1,a2,a3,a4,a5,a6,a7)
ByteMux (i0, [d20,d21,d22,d23,d24,d25,d26,d27],
             [d30,d31,d32,d33,d34,d35,d36,d37])
        (b0,b1,b2,b3,b4,b5,b6,b7)
ByteMux (i1, [a0,a1,a2,a3,a4,a5,a6,a7],
             [b0,b1,b2,b3,b4,b5,b6,b7])
        (o0,o1,o2,o3,o4,o5,o6,o7)

@8ByteMux
: [i0,i1,i2], [d00,d01,d02,d03,d04,d05,d06,d07],
              [d10,d11,d12,d13,d14,d15,d16,d17],
              [d20,d21,d22,d23,d24,d25,d26,d27],
              [d30,d31,d32,d33,d34,d35,d36,d37],
              [d40,d41,d42,d43,d44,d45,d46,d47],
              [d50,d51,d52,d53,d54,d55,d56,d57],
              [d60,d61,d62,d63,d64,d65,d66,d67],
              [d70,d71,d72,d73,d74,d75,d76,d77];
: [o0,o1,o2,o3,o4,o5,o6,o7];
: [a0,a1,a2,a3,a4,a5,a6,a7], [b0,b1,b2,b3,b4,b5,b6,b7];
4ByteMux ([i0, i1], [d00,d01,d02,d03,d04,d05,d06,d07],
                    [d10,d11,d12,d13,d14,d15,d16,d17],
                    [d20,d21,d22,d23,d24,d25,d26,d27],
                    [d30,d31,d32,d33,d34,d35,d36,d37])
          (a0,a1,a2,a3,a4,a5,a6,a7)
4ByteMux ([i0, i1], [d40,d41,d42,d43,d44,d45,d46,d47],
                    [d50,d51,d52,d53,d54,d55,d56,d57],
                    [d60,d61,d62,d63,d64,d65,d66,d67],
                    [d70,d71,d72,d73,d74,d75,d76,d77])
          (b0,b1,b2,b3,b4,b5,b6,b7)
ByteMux (i2, [a0,a1,a2,a3,a4,a5,a6,a7],
             [b0,b1,b2,b3,b4,b5,b6,b7])
        (o0,o1,o2,o3,o4,o5,o6,o7)

@FByteMux
: [i0,i1,i2,i3], [d00,d01,d02,d03,d04,d05,d06,d07],
                 [d10,d11,d12,d13,d14,d15,d16,d17],
                 [d20,d21,d22,d23,d24,d25,d26,d27],
                 [d30,d31,d32,d33,d34,d35,d36,d37],
                 [d40,d41,d42,d43,d44,d45,d46,d47],
                 [d50,d51,d52,d53,d54,d55,d56,d57],
                 [d60,d61,d62,d63,d64,d65,d66,d67],
                 [d70,d71,d72,d73,d74,d75,d76,d77],
                 [d80,d81,d82,d83,d84,d85,d86,d87],
                 [d90,d91,d92,d93,d94,d95,d96,d97],
                 [dA0,dA1,dA2,dA3,dA4,dA5,dA6,dA7],
                 [dB0,dB1,dB2,dB3,dB4,dB5,dB6,dB7],
                 [dC0,dC1,dC2,dC3,dC4,dC5,dC6,dC7],
                 [dD0,dD1,dD2,dD3,dD4,dD5,dD6,dD7],
                 [dE0,dE1,dE2,dE3,dE4,dE5,dE6,dE7],
                 [dF0,dF1,dF2,dF3,dF4,dF5,dF6,dF7];
: [o0,o1,o2,o3,o4,o5,o6,o7];
: [a0,a1,a2,a3,a4,a5,a6,a7], [b0,b1,b2,b3,b4,b5,b6,b7];
8ByteMux ([i0, i1, i2], [d00,d01,d02,d03,d04,d05,d06,d07],
                        [d10,d11,d12,d13,d14,d15,d16,d17],
                        [d20,d21,d22,d23,d24,d25,d26,d27],
                        [d30,d31,d32,d33,d34,d35,d36,d37],
                        [d40,d41,d42,d43,d44,d45,d46,d47],
                        [d50,d51,d52,d53,d54,d55,d56,d57],
                        [d60,d61,d62,d63,d64,d65,d66,d67],
                        [d70,d71,d72,d73,d74,d75,d76,d77])
          (a0,a1,a2,a3,a4,a5,a6,a7)
8ByteMux ([i0, i1, i2], [d80,d81,d82,d83,d84,d85,d86,d87],
                        [d90,d91,d92,d93,d94,d95,d96,d97],
                        [dA0,dA1,dA2,dA3,dA4,dA5,dA6,dA7],
                        [dB0,dB1,dB2,dB3,dB4,dB5,dB6,dB7],
                        [dC0,dC1,dC2,dC3,dC4,dC5,dC6,dC7],
                        [dD0,dD1,dD2,dD3,dD4,dD5,dD6,dD7],
                        [dE0,dE1,dE2,dE3,dE4,dE5,dE6,dE7],
                        [dF0,dF1,dF2,dF3,dF4,dF5,dF6,dF7])
          (b0,b1,b2,b3,b4,b5,b6,b7)
ByteMux (i3, [a0,a1,a2,a3,a4,a5,a6,a7],
             [b0,b1,b2,b3,b4,b5,b6,b7])
        (o0,o1,o2,o3,o4,o5,o6,o7)

@RisingEdge
: in;
: pulse;
: bar;
NOT (in)      (bar)
AND (in, bar) (pulse)

@Delay
: in;
: out;
: d0,d1,d2,d3,d4,d5,d6,d7;
OR (in) (d0)
OR (d0) (d1)
OR (d1) (d2)
OR (d2) (d3)
OR (d3) (d4)
OR (d4) (d5)
OR (d5) (d6)
OR (d6) (d7)
OR (d7) (out)