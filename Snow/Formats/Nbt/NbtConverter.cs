using Snow.Formats.Nbt.Values;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Snow.Formats.Nbt
{
    internal class NbtConverter
    {
        public static NbtCompoundTag Convert(string path, bool debug)
        {
            NbtCompoundTag root = new NbtCompoundTag();
            root.AddFileHeader = debug;

            // Split the text in lines and remove tabs
            string[] lines = File.ReadAllText(path).Replace("\t", string.Empty).Split('\n');
            List<NbtTag> tags = new List<NbtTag> { root };


            // Start and end one early cuz we dont care for the root tag
            for (int i = 1; i < lines.Length - 1; i++)
            {

                string line = lines[i];


                int split = -1;
                bool inTag = false;
                for (int m = 0; m < line.Length; m++)
                {
                    char c = line[m];

                    if (c == '\"')
                    {
                        if (line[m - 1] == '\\')
                            continue;

                        if (!inTag)
                        {
                            inTag = true;
                            continue;
                        }
                        else
                        {
                            inTag = false;
                        }
                    }

                    if (inTag) continue;

                    if (c == ':')
                    {
                        split = m;
                        break;
                    }
                }

                string value;
                string key = null;

                if (split == -1)
                {
                    value = line;
                }
                else
                {
                    value = line.Substring(split + 2);
                    key = line.Substring(0, split);
                }


                // Cleaning

                if (key != null) key = key.Trim();
                value = value.Trim();


                if (value != null && value.EndsWith(","))
                {
                    value = value.Remove(value.Length - 1);
                }

                if (key != null && key.EndsWith("\"") && key.Length - 2 > 1)
                {
                    key = key.Remove(0, 1);
                    key = key.Remove(key.Length - 1);
                }

                if (value.StartsWith("{"))
                {
                    NbtCompoundTag nbtCompoundTag = new NbtCompoundTag();

                    if (tags.Last().type == 0x0a)
                    {
                        ((NbtCompoundTag)tags.Last()).AddField(key, nbtCompoundTag);
                        tags.Add(nbtCompoundTag);
                        continue;
                    }

                    if (tags.Last().type == 0x09)
                    {
                        ((NbtListTag)tags.Last()).tags.Add(nbtCompoundTag);
                        tags.Add(nbtCompoundTag);
                        continue;
                    }
                }

                if (value.StartsWith("["))
                {
                    NbtListTag nbtList = new NbtListTag();

                    if (tags.Last().type == 0x0a)
                    {
                        ((NbtCompoundTag)tags.Last()).AddField(key, nbtList);
                        tags.Add(nbtList);
                        continue;
                    }

                    if (tags.Last().type == 0x09)
                    {
                        ((NbtListTag)tags.Last()).tags.Add(nbtList);
                        tags.Add(nbtList);
                        continue;
                    }
                }

                if (value.StartsWith("}") || value.StartsWith("]"))
                {
                    tags.RemoveAt(tags.Count - 1);
                    continue;
                }

                if (value.StartsWith("\""))
                {
                    value = value.Remove(0, 1);
                    value = value.Remove(value.Length - 1);

                    NbtStringTag stringTag = new NbtStringTag(value);

                    AddToLast(tags, key, stringTag);
                    continue;
                }


                if (value.Contains('.'))
                {

                    if (value.EndsWith("f"))
                    {
                        value = value.Substring(0, value.Length - 1);


                        NbtFloatTag tag = new NbtFloatTag((float)decimal.Parse(value));
                        AddToLast(tags, key, tag);

                        continue;
                    }

                    NbtDoubleTag doubleTag = new NbtDoubleTag((double)decimal.Parse(value));
                    AddToLast(tags, key, doubleTag);
                    continue;
                }

                // Int, Byte

                if (value.Length != 1 && value[1] == 'x')
                {
                    value = value.Substring(2);
                    NbtByteTag tag = new NbtByteTag(byte.Parse(value));
                    AddToLast(tags, key, tag);
                    continue;
                }
                else
                {
                    NbtIntTag intTag = new NbtIntTag(int.Parse(value));
                    AddToLast(tags, key, intTag);
                    continue;
                }


            }

            return root;
        }

        static void AddToLast(List<NbtTag> tags, string key, NbtTag tag)
        {
            if (tags.Last().type == 0x0a)
            {
                ((NbtCompoundTag)tags.Last()).AddField(key, tag);
            }

            if (tags.Last().type == 0x09)
            {
                ((NbtListTag)tags.Last()).tags.Add(tag);
            }
        }
    }
}
