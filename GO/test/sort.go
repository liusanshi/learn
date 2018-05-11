package test

type Tree struct {
	value int
	left, right *Tree
}

func Sort(values []int){
	var root *Tree
	for _, i := range values {
		root = add(root, i)
	}
	appendValues(values[:0], root)
}

//中序遍历树形将顺序输出来
func appendValues(values []int, root *Tree) []int{
	if root != nil {
		values = appendValues(values, root.left)
		values = append(values, root.value)
		values = appendValues(values, root.right)
	}
	return values
}

//构建树 小于父节点的放左边，大于等于的放右边 如果逆序这里修改方向即可
func add(t *Tree, i int) *Tree{
	if t == nil {
		t = new(Tree)
		t.value = i
		return t
	}
	if t.value > i {
		t.left = add(t.left, i)
	} else {
		t.right = add(t.right, i)
	}
	return t
}