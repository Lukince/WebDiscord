using System;
using System.Linq;

namespace WebDiscord
{
    public class EngToKo
    {
        #region Const Values
        const int FINAL = 28;
        const int NEUTRAL = 21;
        const int INITAL = FINAL * NEUTRAL;

        const int INITAL_COUNT = 19;
        const int NEUTRAL_COUNT = 21;
        const int FINAL_COUNT = 28;
        #endregion

        #region Korean Char
        private class KoreanChar
        {
            public KoreanChar(int ini = -1, int neu = -1, int fin = -1)
            {
                if (ini < -1 || ini >= INITAL_COUNT
                    || neu < -1 || neu >= NEUTRAL_COUNT
                    || fin < -1 || fin >= FINAL_COUNT)
                    throw new InvalidOperationException();

                Inital = ini;
                Neutral = neu;
                Final = fin;
            }

            public int Inital { get; set; }

            public int Neutral { get; set; }

            public int Final { get; set; }

            public char ToChar()
            {
                if (Inital == -1)
                {
                    if (Neutral == -1 && Final == -1)
                        return '\0';
                    else if (Neutral == -1 && Final != -1)
                        return (char)('ㄱ' + Final + 1);
                    else if (Neutral != -1 && Final == -1)
                        return (char)('ㅏ' + Neutral);
                    else
                        throw new InvalidOperationException();
                }

                else if (Neutral == -1)
                {
                    if (Final == -1)
                        return InitalKoList[Inital];
                    else
                        throw new InvalidOperationException();
                }

                else if (Final == -1)
                    return (char)('가' + Inital * INITAL + Neutral * FINAL);

                else
                    return (char)('가' + Inital * INITAL + Neutral * FINAL + Final + 1);
            }
        }
        #endregion

        #region Keyboard Interact List
        public static char[] InitalKoList => new char[]
        {
            'ㄱ', 'ㄲ', 'ㄴ', 'ㄷ', 'ㄸ', 'ㄹ', 'ㅁ', 'ㅂ', 'ㅃ', 'ㅅ',
            'ㅆ', 'ㅇ', 'ㅈ', 'ㅉ', 'ㅊ', 'ㅋ', 'ㅌ', 'ㅍ', 'ㅎ'
        };

        public static char[] InitalList => new char[]
        {
            'r', 'R', 's', 'e', 'E', 'f', 'a', 'q', 'Q', 't',
            'T', 'd', 'w', 'W', 'c', 'z', 'x', 'v', 'g'
        };

        public static string[] NeutralList => new string[]
        {
            "k", "o", "i", "O", "j", "p", "u", "P", "h", "hk", "ho",
            "hl", "y", "n", "nj", "np", "nl", "b", "m", "ml", "l"
        };

        public static string[] FinalList => new string[]
        {
            "r", "R", "rt", "s", "sw", "sg", "e", "E", "f", "fr", "fa", "fq",
            "ft", "fx", "fv", "fg", "a", "q", "Q", "qt", "t", "T", "d", "w",
            "W", "c", "z", "x", "v", "g"
        };

        public static string[] FinalssList => new string[]
        {
            "r", "R", "rt", "s", "sw", "sg", "e", "f", "fr", "fa", "fq",
            "ft", "fx", "fv", "fg", "a", "q", "qt", "t", "T", "d", "w",
            "c", "z", "x", "v", "g"
        };

        public static string[] FinalsList => new string[]
        {
            "rt", "sw", "sg", "fr", "fa", "fq", "ft", "fx", "fv", "fg", "qt"
        };
        #endregion

        #region To Korean
        public static string ToKorean(string eng)
        {
            string output = string.Empty;

            char[] c = eng.ToCharArray();

            bool extract = false;
            KoreanChar kchar = new KoreanChar();

            for (int i = 0; i <= c.Length; i++)
            {
                if (i == c.Length)
                {
                    output += kchar.ToChar().ToString();
                    continue;
                }

                string s = string.Empty;

                if (i + 1 < c.Length)
                    s = c[i].ToString() + c[i + 1].ToString();

                if (InitalList.Contains(c[i]))
                {
                    if (FinalsList.Contains(s))
                    {
                        if (s == "E" || s == "Q" || s == "W")
                            i--;

                        if (i + 2 < c.Length && kchar.Inital != -1)
                        {
                            if (NeutralList.Contains(c[i + 2].ToString()))
                            {
                                kchar.Final = Array.IndexOf(FinalssList, c[i].ToString());
                                Extract();
                                continue;
                            }
                        }

                        kchar.Final = Array.IndexOf(FinalssList, s);
                        extract = true;
                        i += s.Length - 1;
                    }
                    else if (kchar.Inital != -1)
                    {
                        if (kchar.Neutral == -1)
                        {
                            Extract();
                            kchar.Inital = Array.IndexOf(InitalList, c[i]);
                        }
                        else if (FinalList.Contains(c[i].ToString()))
                        {
                            if (i + 1 < c.Length)
                            {
                                if (NeutralList.Contains(c[i + 1].ToString()))
                                {
                                    i--;
                                    Extract();
                                    continue;
                                }
                            }

                            kchar.Final = Array.IndexOf(FinalssList, c[i].ToString());
                        }
                        else if (FinalList.Contains(s))
                        {
                            if (i + 2 < c.Length)
                            {
                                if (NeutralList.Contains(c[i + 2].ToString()))
                                {
                                    i--;
                                    Extract();
                                    continue;
                                }
                            }

                            kchar.Final = Array.IndexOf(FinalssList, s);
                        }

                        extract = true;
                    }
                    else
                        kchar.Inital = Array.IndexOf(InitalList, c[i]);
                }
                else if (NeutralList.Contains(c[i].ToString()))
                {
                    if (NeutralList.Contains(s))
                    {
                        kchar.Neutral = Array.IndexOf(NeutralList, s);
                        i += s.Length - 1;
                    }
                    else
                        kchar.Neutral = Array.IndexOf(NeutralList, c[i].ToString());

                    if (kchar.Inital == -1)
                        extract = true;
                    else
                        extract = false;
                }
                else
                {
                    Extract();
                    output += c[i];
                    continue;
                }

                if (extract)
                    Extract();
            }

            return output;

            void Extract()
            {
                var c = kchar.ToChar();

                if (c != '\0')
                    output += c.ToString();

                kchar = new KoreanChar();
                extract = false;
            }
        }
        #endregion
    }
}