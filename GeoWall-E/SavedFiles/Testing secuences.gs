//sec = {1 ... 3} where x > 5;
sec = {1 ... 3} + {5,2,6} + undefined;

print {1,2,3} + {4,5,6};

c,d,e,f,g = sec where (x > 2);
print sec where (x >= 2) "sec";

print c "c";
print d "d";
print e "e";
print f "f";
print g "g";

print count(sec) "count sec";