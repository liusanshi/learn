(function(target) {

    var Enumerable = target.Enumerable = {};

    Enumerable.from = function(obj) {
        if (!obj) return Enumerable.empty();
        if (obj instanceof iterator) return obj;
        if (typeof obj == Utils.types.Number || typeof obj == Utils.types.Boolean) {
            return Enumerable.$return(obj, 1);
        }
        else if (typeof obj == Utils.types.String) {
            var index = -1, len = obj.length;
            return new iterator(Utils.emptyFunction,
                function() {
                    return (++index < len) ? this.yield(obj.charAt(index)) : false;
                }
                , Utils.emptyFunction);
        }
        return new iterator(Utils.emptyFunction, Utils.defaultGetNext(obj), Utils.emptyFunction);
    };
    Enumerable.range = function(min, length, step) {
        step = step || 1;
        var curr = 0 - step, index = -1;
        return new iterator(Utils.emptyFunction, function() {
            curr += step;
            return (++index < length) ? this.yield(curr + min) : false;
        }, Utils.emptyFunction);
    };
    Enumerable.$return = function(item) {
        var isReturn = false;
        return new iterator(Utils.emptyFunction, function() {
            if (isReturn) {
                return false;
            } else {
                isReturn = true;
                return this.yield(item);
            }
        }, Utils.emptyFunction);
    };
    Enumerable.empty = function() {
        return new iterator(Utils.emptyFunction, function() { return false; }, Utils.emptyFunction)
    };

    var Utils = {
        defaultComparer: function(fun) {
            var defaultfunc = function(a, b) { return a > b ? 1 : (a === b ? 0 : -1); };
            fun = this.lambdaFunction(fun) || fun || Utils.isFunction(fun) || defaultfunc;
            return fun;
        },
        defaultEqual: function(fun) {
            var defaultfunc = function(a, b) { return a == b; };
            fun = this.lambdaFunction(fun) || fun || Utils.isFunction(fun) || defaultfunc;
            return fun;
        },
        defaultPredicate: function(fun) {
            var defaultfunc = function() { return true; };
            fun = this.lambdaFunction(fun) || fun || Utils.isFunction(fun) || defaultfunc;
            return fun;
        },
        defaultSelector: function(fun) {
            var defaultfunc = function(a) { return a; };
            fun = this.lambdaFunction(fun) || fun || Utils.isFunction(fun) || defaultfunc;
            return fun;
        },
        defaultCollectionSelector: function(fun) {
            var defaultfunc = function(a) { return Enumerable.$return(a); };
            fun = this.lambdaFunction(fun) || fun || Utils.isFunction(fun) || defaultfunc;
            return fun;
        },
        emptyFunction: function() { },
        defaultGetNext: function(data) {
            var __data = data || [], __index = -1, __len = 0;
            if (typeof __data.length !== Utils.types.Number) {
                __data = getKeyValuePair(data);
            }
            __index = -1;
            __len = __data.length;

            return function() {
                if (__len > ++__index) {
                    this.yield(__data[__index]);
                    return true;
                }
                return false;
            }
            function getKeyValuePair(dic) {
                var result = [];
                for (var i in dic) {
                    if (dic.hasOwnProperty(i) && !Utils.isFunction(dic[i])) {
                        result.push({ key: i, value: dic[i] });
                    }
                };
                return result;
            }
        },
        lambdaCache: {},
        lambdaFunction: function(body) {
            if (typeof body !== Utils.types.String) return false;
            var func = this.lambdaCache[body];
            if (!func) {
                func = null;
                var s = body.split("=>");
                if (s.length != 2) return false;
                //参数处理
                var args = s[0].replace("(", "").replace(")", "").replace(/(^\s*)|(\s*$)/g, "");
                //函数体处理
                var db = [];
                db.push('try{');
                if ((' ' + s[1]).indexOf('return') > -1) {
                    db.push(s[1]);
                } else {
                    db.push('return ');
                    db.push(s[1]);
                }
                db.push('}catch(e){alert(e)}');
                func = new Function(args, db.join(' '));
                this.lambdaCache[body] = func;
            }
            return func;
        },
        indexOf: function(arr, item, equal) {
            for (var i = arr.length - 1; i >= 0; i--) {
                if (equal(item, arr[i])) {
                    return i;
                }
            };
            return -1;
        },
        quickSort: function(arr, left, right, comparerfun) {//快速排序
            if (left >= right) return;
            var mid = arr[left + ((right - left) >> 1)];
            var i = left - 1, j = right + 1;
            while (true) {
                while (comparerfun(arr[++i], mid) < 0);
                while (comparerfun(arr[--j], mid) > 0);
                if (i >= j) {
                    break;
                }
                var temp = arr[i];
                arr[i] = arr[j];
                arr[j] = temp;
            }
            Utils.quickSort(arr, left, i - 1, comparerfun);
            Utils.quickSort(arr, j + 1, right, comparerfun);
        },
        sort: function(iterator1, comparerfun) {
            var arr = [];
            while (iterator1.moveNext()) {
                arr.push(iterator1.current());
            }
            var left = 0, right = arr.length - 1;
            Utils.quickSort(arr, left, right, comparerfun);
            return arr;
        },
        isFunction: function(args) {
            return typeof args === 'function';
        },
        types: {
            Boolean: typeof true,
            Number: typeof 0,
            String: typeof "",
            Object: typeof {},
            Undefined: typeof undefined,
            Function: typeof function() { }
        },
        dispose: function(obj) {
            if (obj && obj.dispose) {
                obj.dispose();
            }
        }
    };

    var State = { Before: 0, Running: 1, After: 2 }
    //迭代器
    function iterator(init, getnext, _dispose) {
        var current = null,
            state = State.Before;

        this.yield = function(val) { current = val; return true; }
        this.current = function() { return current; }
        this.moveNext = function() {
            try {
                switch (state) {
                    case State.Before:
                        state = State.Running;
                        init();
                    case State.Running:
                        if (getnext.call(this)) {
                            return true;
                        } else {
                            this.dispose();
                            return false;
                        }
                    case State.After:
                        return false;
                }
            } catch (e) {
                this.dispose();
                throw e;
            }
        };
        this.dispose = function() {
            if (state != State.Running) return;

            try { if (_dispose) { _dispose(); } }
            finally { state = State.After; }
        };

    }
    iterator.prototype.foreach = function(action) {
        action = Utils.defaultSelector(action);
        var index = 0;
        while (this.moveNext()) {
            if (action(this.current(), index++) === false) break;
        }
    };
    iterator.prototype.where = function(fun) {
        fun = Utils.defaultPredicate(fun);
        var scource = this;
        return new iterator(Utils.emptyFunction,
            function() {
                while (scource.moveNext()) {
                    if (fun(scource.current())) {
                        return this.yield(scource.current());
                    }
                }
                return false;
            },
            Utils.emptyFunction);
    };
    iterator.prototype.select = function(selector) {
        selector = Utils.defaultSelector(selector);
        var scource = this, index = 0;
        return new iterator(Utils.emptyFunction,
            function() {
                while (scource.moveNext()) {
                    return this.yield(selector(scource.current(), index++));
                }
                return false;
            },
            Utils.emptyFunction);
    };
    iterator.prototype.selectMany = function(collectionSelector, resultSelector) {
        resultSelector = Utils.defaultSelector(resultSelector);
        collectionSelector = Utils.defaultSelector(collectionSelector);
        var middleEnumerator = undefined, scource = this, index = 0;
        return new iterator(Utils.emptyFunction,
            function() {
                if (middleEnumerator === undefined) {
                    if (!scource.moveNext()) return false;
                }
                do {
                    if (middleEnumerator == null) {
                        var middleSeq = collectionSelector(scource.current(), index++);
                        middleEnumerator = Enumerable.from(middleSeq);
                    }
                    if (middleEnumerator.moveNext()) {
                        return this.yield(resultSelector(scource.current(), middleEnumerator.current()));
                    }
                    Utils.dispose(middleEnumerator);
                    middleEnumerator = null;
                } while (scource.moveNext())
                return false;
            },
            function() {
                try { Utils.dispose(scource); }
                finally { Utils.dispose(middleEnumerator); }
            });
    };
    iterator.prototype.defaultIfEmpty = function(defaultvalue) {
        var scource = this, isFirst = true;
        return new iterator(
            Utils.emptyFunction,
            function() {
                if (scource.moveNext()) {
                    isFirst = false;
                    return this.yield(scource.current());
                } else if (isFirst) {
                    isFirst = false;
                    return this.yield(defaultvalue);
                }
                return false;
            },
            Utils.emptyFunction
        )
    };
    iterator.prototype.toArray = function() {
        var temp = [];
        this.foreach(function(item) {
            temp.push(item)
        });
        return temp;
    };
    iterator.prototype.firstOrDefault = function(predicate, defaultvalue) {
        predicate = Utils.defaultPredicate(predicate);
        var curr = defaultvalue;
        while (this.moveNext()) {
            if (predicate(this.current())) {
                curr = this.current();
                break;
            }
        }
        return curr;
    };
    iterator.prototype.lastOrDefault = function(predicate, defaultvalue) {
        predicate = Utils.defaultPredicate(predicate);
        var curr = defaultvalue;
        while (this.moveNext()) {
            if (predicate(this.current())) {
                curr = this.current();
            }
        }
        return curr;
    };
    iterator.prototype.singleOrDefault = function(predicate, defaultvalue) {
        predicate = Utils.defaultPredicate(predicate);
        var curr = defaultvalue, found = false;
        while (this.moveNext()) {
            if (predicate(this.current())) {
                if (!found) {
                    found = true;
                    curr = this.current();
                } else {
                    throw new Error("Single:sequence contains more than one element.");
                }
            }
        }
        return curr;
    };
    iterator.prototype.elementAtOrDefault = function(index) {
        var el = null;
        this.foreach(function(item, i) {
            if (index == i) {
                el = item;
                return false;
            }
        })
        return el;
    };
    iterator.prototype.all = function(predicate) {
        predicate = Utils.defaultPredicate(predicate);
        var result = true;
        this.foreach(function(item) {
            if (!predicate(item)) {
                result = false;
                return false;
            }
        });
        return result;
    };
    iterator.prototype.any = function(fun) {
        fun = Utils.defaultPredicate(fun);
        var result = false;
        this.foreach(function(item) {
            if (fun(item)) {
                result = true;
                return false;
            }
        });
        return result;
    };
    iterator.prototype.average = function() {
        var count = 0, sum = 0;
        this.foreach(function(item) {
            if (!isNaN(item)) {
                sum += item;
                count++;
            }
        });
        return count !== 0 ? sum / count : sum;
    };
    iterator.prototype.count = function(fun) {
        fun = Utils.defaultPredicate(fun);
        var count = 0;
        this.foreach(function(item) {
            if (fun(item)) {
                count++;
            }
        });
        return count;
    };
    iterator.prototype.max = function(fun) {
        fun = Utils.defaultSelector(fun);
        var max = null, cur = null;
        this.foreach(function(item) {
            if (max == null) {
                max = fun(item);
            } else {
                cur = fun(item);
                if (max < cur) {
                    max = cur;
                }
            }
        });
        return max;
    };
    iterator.prototype.min = function(fun) {
        fun = Utils.defaultSelector(fun);
        var min = null, cur = null;
        this.foreach(function(item) {
            if (min == null) {
                min = fun(item);
            } else {
                cur = fun(item);
                if (min > cur) {
                    min = cur;
                }
            }
        });
        return min;
    };
    iterator.prototype.distinct = function(equal) {
        return this.except(Enumerable.empty(), equal)
    };
    iterator.prototype.except = function(second, equal) {
        equal = Utils.defaultEqual(equal);
        var scource = this, keys;
        return new iterator(function() {
            keys = [];
            Enumerable.from(second).foreach(function(key) { keys.push(key); });
        },
            function() {
                while (scource.moveNext()) {
                    var current = scource.current();
                    if (Utils.indexOf(keys, current, equal) === -1) {
                        keys.push(current);
                        return this.yield(current);
                    }
                }
                return false;
            },
            Utils.emptyFunction);
    };
    iterator.prototype.concat = function(second) {
        var firstIterator = this, secondIterator;
        return new iterator(
            Utils.emptyFunction,
            function() {
                if (secondIterator == null) {
                    if (firstIterator.moveNext())
                        return this.yield(firstIterator.current());
                    secondIterator = Enumerable.from(second);
                }
                if (secondIterator.moveNext())
                    return this.yield(secondIterator.current());
                return false;
            },
            function() {
                try { Utils.dispose(firstIterator); }
                finally { Utils.dispose(secondIterator); }
            });
    };
    iterator.prototype.repeat = function(count) {
        var scource = this, index = 1, cache = [], i = 0, len = 0;
        return new iterator(Utils.emptyFunction,
            function() {
                while (index <= count) {
                    while (scource.moveNext()) {
                        cache.push(scource.current());
                        return this.yield(scource.current());
                    }
                    if (len == 0) index++;
                    len = cache.length;
                    if (i < len) {
                        return this.yield(cache[i++]);
                    }
                    i = 0;
                    index++;
                }
                return false;
            }
            , function() {
                cache = null;
                scource.dispose();
            });
    };
    iterator.prototype.union = function(second, equal) {
        equal = Utils.defaultEqual(equal);
        var firstEnumerator = this,
            secondEnumerator,
            keys;
        return new iterator(
                function() {
                    keys = [];
                },
                function() {
                    var current;
                    if (secondEnumerator === undefined) {
                        while (firstEnumerator.moveNext()) {
                            current = firstEnumerator.current();
                            if (Utils.indexOf(keys, current, equal) == -1) {
                                keys.push(current);
                                return this.yield(current);
                            }
                        }
                        secondEnumerator = Enumerable.from(second);
                    }
                    while (secondEnumerator.moveNext()) {
                        current = secondEnumerator.current();
                        if (Utils.indexOf(keys, current, equal) == -1) {
                            keys.push(current);
                            return this.yield(current);
                        }
                    }
                    return false;
                },
                function() {
                    try { Utils.dispose(firstEnumerator); }
                    finally { Utils.dispose(secondEnumerator); }
                });
    };
    iterator.prototype.toLookup = function(keySelector, elementSelector, compareSelector) {
        keySelector = Utils.defaultSelector(keySelector);
        elementSelector = Utils.defaultSelector(elementSelector);
        compareSelector = Utils.defaultComparer(compareSelector);

        var dict = new Dictionary(compareSelector);
        this.foreach(function(x) {
            var key = keySelector(x);
            var element = elementSelector(x);

            var array = dict.Get(key);
            if (array !== undefined) array.push(element);
            else dict.Add(key, [element]);
        });
        return new Lookup(dict);
    };
    iterator.prototype.groupBy = function(keySelector, elementSelector, resultfun, compareSelector) {
        keySelector = Utils.defaultSelector(keySelector);
        elementSelector = Utils.defaultSelector(elementSelector);
        resultfun = Utils.defaultSelector(resultfun);
        compareSelector = Utils.defaultSelector(compareSelector);
        var scource = this, enumerator = null;
        return new iterator(
            function() {
                enumerator = scource.toLookup(keySelector, elementSelector, compareSelector).ToEnumerable();
            }, function() {
                while (enumerator.moveNext()) {
                    return this.yield(resultfun(enumerator.current()));
                }
                return false;
            }, Utils.emptyFunction);
    };
    iterator.prototype.toDictionary = function(keySelector, elementSelector, compareSelector) {
        keySelector = Utils.defaultSelector(keySelector);
        elementSelector = Utils.defaultSelector(elementSelector);
        compareSelector = Utils.defaultSelector(compareSelector);

        var dict = new Dictionary(compareSelector);
        this.foreach(function(x) {
            dict.Add(keySelector(x), elementSelector(x));
        });
        return dict;
    };
    iterator.prototype.orderBy = function(selectkeyfun, comparerfun) {
        return new orderedIterator(this, selectkeyfun, comparerfun, false);
    };
    iterator.prototype.orderByDes = function(selectkeyfun, comparerfun) {
        return new orderedIterator(this, selectkeyfun, comparerfun, true);
    };
    iterator.prototype.reverse = function() {
        var scource = this, reversedata = null, len = 0;
        return new iterator(
            function() {
                reversedata = scource.toArray();
                len = reversedata.length;
            },
            function() {
                return (len > 0) ? this.yield(reversedata[--len]) : false;
            },
            Utils.emptyFunction);
    };
    iterator.prototype.intersect = function(second, equal) { //获取相同的
        equal = Utils.defaultEqual(equal);
        var scource = this,
        keys = null, outs = null;
        return new iterator(
            function() {
                keys = [];
                Enumerable.from(second).foreach(function(key) { keys.push(key); });
                outs = [];
            },
            function() {
                while (scource.moveNext()) {
                    var current = scource.current();
                    if (Utils.indexOf(outs, current, equal) == -1 &&
                        Utils.indexOf(keys, current, equal) > -1) {
                        outs.push(current);
                        return this.yield(current);
                    }
                }
                return false;
            },
            Utils.emptyFunction);
    };
    iterator.prototype.join = function(inner, outerKeySelector, innerKeySelector, resultSelector, compareSelector) {
        outerKeySelector = Utils.defaultSelector(outerKeySelector);
        innerKeySelector = Utils.defaultSelector(innerKeySelector);
        resultSelector = Utils.defaultSelector(resultSelector);
        compareSelector = Utils.defaultSelector(compareSelector);
        var outer = this, lookup = null, innerElements = null, innerCount = 0;
        return new iterator(
            function() {
                lookup = Enumerable.from(inner).toLookup(innerKeySelector, null, compareSelector);
            },
            function() {
                while (true) {
                    if (innerElements != null) {
                        var el = innerElements[innerCount++];
                        if (el !== undefined) {
                            return this.yield(resultSelector(el));
                        }
                        innerElements = null;
                        innerCount = 0;
                    }
                    if (outer.moveNext()) {
                        var key = outerKeySelector(outer.current());
                        innerElements = lookup.Get(key).toArray();
                    } else {
                        return false;
                    }
                }
                return false;
            },
            Utils.emptyFunction);
    };
    iterator.prototype.groupJoin = function(inner, outerKeySelector, innerKeySelector, resultSelector, compareSelector) {
        outerKeySelector = Utils.defaultSelector(outerKeySelector);
        innerKeySelector = Utils.defaultSelector(innerKeySelector);
        resultSelector = Utils.defaultSelector(resultSelector);
        compareSelector = Utils.defaultSelector(compareSelector);
        var lookup = null, scource = this;
        return new iterator(
            function() {
                lookup = Enumerable.from(inner).toLookup(innerKeySelector, null, compareSelector)
            },
            function() {
                if (scource.moveNext()) {
                    var innerElement = lookup.Get(outerKeySelector(scource.current()));
                    return this.yield(resultSelector(scource.current(), innerElement));
                }
                return false;
            }, Utils.emptyFunction);
    };
    iterator.prototype.sequenceEqual = function(second, compareSelector) {
        compareSelector = Utils.defaultSelector(compareSelector);
        var firstEnumerator = this;
        try {
            var secondEnumerator = Enumerable.from(second);
            try {
                while (firstEnumerator.moveNext()) {
                    if (!secondEnumerator.moveNext()
                        || compareSelector(firstEnumerator.current()) !== compareSelector(secondEnumerator.current())) {
                        return false;
                    }
                }
                if (secondEnumerator.moveNext()) return false;
                return true;
            } finally {
                Utils.dispose(secondEnumerator);
            }
        } finally {
            Utils.dispose(firstEnumerator);
        }
    };
    iterator.prototype.toString = function(separator, selector) {
        if (separator == null) separator = "";
        selector = Utils.defaultSelector(selector);
        return this.select(selector).toArray().join(separator);
    };
    iterator.prototype.skip = function(count) {
        var source = this, count = count || 0, index = 0;
        return new iterator(
            function() { while (index++ < count && source.moveNext()); },
            function() { if (source.moveNext()) { return this.yield(source.current()); } return false; },
            Utils.emptyFunction
            );
    };
    iterator.prototype.skipWhile = function(predicate) {
        predicate = Utils.defaultPredicate(predicate);
        var source = this, isEnd = false, index = 0;
        return new iterator(
            Utils.emptyFunction,
            function() {
                while (!isEnd) {
                    if (source.moveNext()) {
                        if (!predicate(source.current(), index++)) {
                            isEnd = true;
                            return this.yield(source.current());
                        }
                        continue;
                    } else {
                        return false;
                    }
                }
                if (source.moveNext()) {
                    return this.yield(source.current());
                } else {
                    return false;
                }
            },
            Utils.emptyFunction
            );
    };
    iterator.prototype.take = function(count) {
        var source = this, count = count || 0, index = 0;
        return new iterator(
            Utils.emptyFunction,
            function() { if (index++ < count && source.moveNext()) { return this.yield(source.current()); } return false; },
            Utils.emptyFunction
            );
    };
    iterator.prototype.takeWhile = function(predicate) {
        predicate = Utils.defaultPredicate(predicate);
        var source = this, index = 0;
        return new iterator(
            Utils.emptyFunction,
            function() {
                if (source.moveNext() && predicate(source.current(), index++)) {
                    return this.yield(source.current());
                }
                return false;
            },
            Utils.emptyFunction
            );
    };

    // for thenBy \ thenByDes
    function orderedIterator(source, keySelector, comparer, descending, parent) {
        this.source = source;
        this.keySelector = Utils.defaultSelector(keySelector);
        this.descending = descending;
        this.comparer = Utils.defaultComparer(comparer);
        this.parent = parent;
        var self = this, buffer, indexs, index = 0, len = 0;

        iterator.call(self, function() {
            buffer = [];
            indexs = [];
            var i = 0;
            while (source.moveNext()) {
                buffer.push(source.current());
                indexs.push(i++);
            }
            len = buffer.length;
            var sortContext = SortContext.create(self, null);
            sortContext.generateKeys(buffer);

            //Utils.quickSort(indexs, 0, len - 1, function(a, b) { return sortContext.compare(a, b); }); //快速排序
            indexs.sort(function(a, b) { return sortContext.compare(a, b); }); //使用原生的排序，性能好
        },
        function() {
            return (index < len) ? this.yield(buffer[indexs[index++]]) : false;
        }
        , Utils.emptyFunction)
    }
    orderedIterator.prototype = new iterator();
    orderedIterator.prototype.constructor = orderedIterator;
    orderedIterator.prototype.createOrderedIterator = function(keySelector, comparer, descending) {
        return new orderedIterator(this.source, keySelector, comparer, descending, this);
    };
    orderedIterator.prototype.thenBy = function(keySelector, comparer) {
        return this.createOrderedIterator(keySelector, comparer, false);
    };
    orderedIterator.prototype.thenByDes = function(keySelector, comparer) {
        return this.createOrderedIterator(keySelector, comparer, true);
    };

    function SortContext(keySelector, comparer, descending, child) {
        this.keySelector = keySelector;
        this.descending = descending;
        this.comparer = comparer;
        this.child = child;
        this.keys = null;
    }
    SortContext.create = function(orderedEnumerable, currentContext) {
        var context = new SortContext(orderedEnumerable.keySelector, orderedEnumerable.comparer,
                                      orderedEnumerable.descending, currentContext);
        if (orderedEnumerable.parent != null) return SortContext.create(orderedEnumerable.parent, context);
        return context;
    }
    SortContext.prototype.generateKeys = function(source) {
        var len = source.length;
        var keySelector = this.keySelector;
        var keys = new Array(len);
        for (var i = 0; i < len; i++) keys[i] = keySelector(source[i]);
        this.keys = keys;

        if (this.child != null) this.child.generateKeys(source);
    };
    SortContext.prototype.compare = function(index1, index2) {
        var comparison = this.comparer(this.keys[index1], this.keys[index2]);

        if (comparison == 0) {
            if (this.child != null) return this.child.compare(index1, index2);
            comparison = this.comparer(index1, index2);
        }

        return (this.descending) ? -comparison : comparison;
    }

    // dictionary = Dictionary<TKey, TValue[]>
    var Lookup = function(dictionary) {
        this.Count = function() {
            return dictionary.Count();
        };
        this.Get = function(key) {
            return Enumerable.from(dictionary.Get(key));
        };
        this.Contains = function(key) {
            return dictionary.Contains(key);
        };
        this.ToEnumerable = function() {
            return dictionary.ToEnumerable().select(function(kvp) {
                return new grouping(kvp.Key, kvp.Value);
            });
        }
    }
    function grouping(key, values) {
        this.key = function() {
            return key;
        }
        iterator.call(this, Utils.emptyFunction, Utils.defaultGetNext(values), Utils.emptyFunction)
    }
    grouping.prototype = new iterator();
    grouping.prototype.constructor = grouping;

    var Dictionary = (function() {
        // static utility methods
        var HasOwnProperty = function(target, key) {
            return Object.prototype.hasOwnProperty.call(target, key);
        }

        var ComputeHashCode = function(obj) {
            if (obj === null) return "null";
            if (obj === undefined) return "undefined";

            return (typeof obj.toString === 'function')
                ? obj.toString()
                : Object.prototype.toString.call(obj);
        }

        // LinkedList for Dictionary
        var HashEntry = function(key, value) {
            this.Key = key;
            this.Value = value;
            this.Prev = null;
            this.Next = null;
        }

        var EntryList = function() {
            this.First = null;
            this.Last = null;
        }
        EntryList.prototype =
        {
            AddLast: function(entry) {
                if (this.Last != null) {
                    this.Last.Next = entry;
                    entry.Prev = this.Last;
                    this.Last = entry;
                }
                else this.First = this.Last = entry;
            },

            Replace: function(entry, newEntry) {
                if (entry.Prev != null) {
                    entry.Prev.Next = newEntry;
                    newEntry.Prev = entry.Prev;
                }
                else this.First = newEntry;

                if (entry.Next != null) {
                    entry.Next.Prev = newEntry;
                    newEntry.Next = entry.Next;
                }
                else this.Last = newEntry;

            },

            Remove: function(entry) {
                if (entry.Prev != null) entry.Prev.Next = entry.Next;
                else this.First = entry.Next;

                if (entry.Next != null) entry.Next.Prev = entry.Prev;
                else this.Last = entry.Prev;
            }
        }

        // Overload:function()
        // Overload:function(compareSelector)
        var Dictionary = function(compareSelector) {
            this.count = 0;
            this.entryList = new EntryList();
            this.buckets = {}; // as Dictionary<string,List<object>>
            this.compareSelector = (compareSelector == null) ? function(i) { return i; } : compareSelector;
        }

        Dictionary.prototype =
        {
            Add: function(key, value) {
                var compareKey = this.compareSelector(key);
                var hash = ComputeHashCode(compareKey);
                var entry = new HashEntry(key, value);
                if (HasOwnProperty(this.buckets, hash)) {
                    var array = this.buckets[hash];
                    for (var i = 0; i < array.length; i++) {
                        if (this.compareSelector(array[i].Key) === compareKey) {
                            this.entryList.Replace(array[i], entry);
                            array[i] = entry;
                            return;
                        }
                    }
                    array.push(entry);
                }
                else {
                    this.buckets[hash] = [entry];
                }
                this.count++;
                this.entryList.AddLast(entry);
            },

            Get: function(key) {
                var compareKey = this.compareSelector(key);
                var hash = ComputeHashCode(compareKey);
                if (!HasOwnProperty(this.buckets, hash)) return undefined;

                var array = this.buckets[hash];
                for (var i = 0; i < array.length; i++) {
                    var entry = array[i];
                    if (this.compareSelector(entry.Key) === compareKey) return entry.Value;
                }
                return undefined;
            },

            Set: function(key, value) {
                var compareKey = this.compareSelector(key);
                var hash = ComputeHashCode(compareKey);
                if (HasOwnProperty(this.buckets, hash)) {
                    var array = this.buckets[hash];
                    for (var i = 0; i < array.length; i++) {
                        if (this.compareSelector(array[i].Key) === compareKey) {
                            var newEntry = new HashEntry(key, value);
                            this.entryList.Replace(array[i], newEntry);
                            array[i] = newEntry;
                            return true;
                        }
                    }
                }
                return false;
            },

            Contains: function(key) {
                var compareKey = this.compareSelector(key);
                var hash = ComputeHashCode(compareKey);
                if (!HasOwnProperty(this.buckets, hash)) return false;

                var array = this.buckets[hash];
                for (var i = 0; i < array.length; i++) {
                    if (this.compareSelector(array[i].Key) === compareKey) return true;
                }
                return false;
            },

            Clear: function() {
                this.count = 0;
                this.buckets = {};
                this.entryList = new EntryList();
            },

            Remove: function(key) {
                var compareKey = this.compareSelector(key);
                var hash = ComputeHashCode(compareKey);
                if (!HasOwnProperty(this.buckets, hash)) return;

                var array = this.buckets[hash];
                for (var i = 0; i < array.length; i++) {
                    if (this.compareSelector(array[i].Key) === compareKey) {
                        this.entryList.Remove(array[i]);
                        array.splice(i, 1);
                        if (array.length == 0) delete this.buckets[hash];
                        this.count--;
                        return;
                    }
                }
            },

            Count: function() {
                return this.count;
            },
            ToEnumerable: function() {
                var self = this;
                var currentEntry;
                return new iterator(
                    function() { currentEntry = self.entryList.First },
                    function() {
                        if (currentEntry != null) {
                            var result = { Key: currentEntry.Key, Value: currentEntry.Value };
                            currentEntry = currentEntry.Next;
                            return this.yield(result);
                        }
                        return false;
                    },
                    Utils.emptyFunction);
            }
        }

        return Dictionary;
    })();
    return Enumerable;
})(window);


