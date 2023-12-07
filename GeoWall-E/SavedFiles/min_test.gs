min2(x, y) =
    if x <= y then
        x
    else y
;

print min2(1,2) "min2 ";

min(s) =
    let
        v, tail = s;
    in
        if tail then
            min2(v, min(tail))
        else
            v
;

print min({3 ... 5}) "min sec ";


take(s, n) =
    if n and s then
        let
            a, tail = s;
            print a;
        in
            {a} + take(tail, n - 1)
      else {}
;

print take({3 ... 10}, 3);
