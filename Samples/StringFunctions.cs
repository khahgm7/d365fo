private boolean StartsWith(str _text, str _part, boolean _useUpperCase = false)
{
    str startPart = subStr(_text, 1, strLen(_part));

    if(_useUpperCase)
            startPart = strUpr(startPart);

    return strUpr(_part) == startPart;
}

private boolean EndsWith(str _text, str _part, boolean _useUpperCase = false)
{
    str endPart = subStr(_text, strLen(_text) - strLen(_part) + 1, strLen(_part));

    if(_useUpperCase)
            endPart = strUpr(endPart);

    return strUpr(_part) == endPart;
}