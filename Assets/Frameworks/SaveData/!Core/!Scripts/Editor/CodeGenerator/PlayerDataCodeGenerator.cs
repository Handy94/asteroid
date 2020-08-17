using HandyPackage.CodeGeneration;
using System.Collections.Generic;

namespace HandyPackage.Editor
{
    public static class PlayerDataCodeGenerator
    {
        public static string GeneratePlayerDataScript(List<PlayerDataEditorData> datas)
        {
            ClassGenerationData classData = new ClassGenerationData
            {
                m_ClassName = "PlayerData",
                m_Namespace = PlayerDataCodeGeneratorConstants.NAMESPACE_GAME,
                m_Usings = new string[] { "UniRx", PlayerDataCodeGeneratorConstants.NAMESPACE_FRAMEWORK },
                m_ClassType = ClassType.Partial
            };

            for (int i = 0; i < datas.Count; i++)
                classData.m_FieldGenerationDatas.Add(PlayerDataFieldGenerator.GeneratePlayerDataField(datas[i]));

            string code = ClassGenerator.CreateClass(classData);
            return code;
        }

        public static string GeneratePlayerDataGetterScript(List<PlayerDataEditorData> datas)
        {
            ClassGenerationData classData = new ClassGenerationData
            {
                m_ClassName = "PlayerDataGetter",
                m_Namespace = PlayerDataCodeGeneratorConstants.NAMESPACE_GAME,
                m_Usings = new string[] { "UniRx", "System", "System.Collections.Generic", PlayerDataCodeGeneratorConstants.NAMESPACE_FRAMEWORK },
                m_ClassType = ClassType.Partial
            };

            for (int i = 0; i < datas.Count; i++)
            {
                if (!datas[i].shouldGenerateGetter)
                    continue;

                classData.m_MethodGenerationDatas.AddRange(PlayerDataMethodGenerator.GenerateGetterMethods(datas[i]));
            }


            string code = ClassGenerator.CreateClass(classData);
            return code;
        }

        public static string GeneratePlayerDataKeysScript(List<PlayerDataEditorData> datas)
        {
            ClassGenerationData classData = new ClassGenerationData
            {
                m_ClassName = "PlayerDataKeys",
                m_Namespace = PlayerDataCodeGeneratorConstants.NAMESPACE_GAME,
                m_ClassType = ClassType.StaticPartial,
                m_Usings = new string[] { PlayerDataCodeGeneratorConstants.NAMESPACE_FRAMEWORK }
            };

            for (int i = 0; i < datas.Count; i++)
                classData.m_FieldGenerationDatas.Add(PlayerDataFieldGenerator.GeneratePlayerDataKeyField(datas[i]));

            string code = ClassGenerator.CreateClass(classData);
            return code;
        }

        public static string GeneratePlayerDataKeyValueGetterScript(List<PlayerDataEditorData> datas, string category)
        {
            ClassGenerationData classData = new ClassGenerationData
            {
                m_ClassName = "PlayerDataKeyValueGetter",
                m_Namespace = PlayerDataCodeGeneratorConstants.NAMESPACE_GAME,
                m_ClassType = ClassType.Partial,
                m_Usings = new string[] { "System", "System.Collections.Generic", PlayerDataCodeGeneratorConstants.NAMESPACE_FRAMEWORK }
            };

            classData.m_MethodGenerationDatas.Add(PlayerDataMethodGenerator.GeneratePlayerDataKeyValueGetterMethod(datas, category));

            string code = ClassGenerator.CreateClass(classData);
            return code;
        }

        public static string GeneratePlayerDataLoaderScript(List<PlayerDataEditorData> datas, string category)
        {
            ClassGenerationData classData = new ClassGenerationData
            {
                m_ClassName = "PlayerDataLoader",
                m_Namespace = PlayerDataCodeGeneratorConstants.NAMESPACE_GAME,
                m_ClassType = ClassType.Partial,
                m_Usings = new string[] { "UniRx", "System", "System.Collections.Generic", PlayerDataCodeGeneratorConstants.NAMESPACE_FRAMEWORK }
            };

            classData.m_MethodGenerationDatas.Add(PlayerDataMethodGenerator.GenerateLoadMethod(datas, category));

            string code = ClassGenerator.CreateClass(classData);
            return code;
        }

        public static string GeneratePlayerDataMutatorScript(List<PlayerDataEditorData> datas)
        {
            ClassGenerationData classData = new ClassGenerationData
            {
                m_ClassName = "PlayerDataMutator",
                m_Namespace = PlayerDataCodeGeneratorConstants.NAMESPACE_GAME,
                m_Usings = new string[] { "UniRx", "System", "System.Collections.Generic", PlayerDataCodeGeneratorConstants.NAMESPACE_FRAMEWORK },
                m_ClassType = ClassType.Partial
            };

            for (int i = 0; i < datas.Count; i++)
            {
                if (!datas[i].shouldGenerateMutator)
                    continue;

                classData.m_MethodGenerationDatas.AddRange(PlayerDataMethodGenerator.GenerateMutatorMethods(datas[i]));
            }


            string code = ClassGenerator.CreateClass(classData);
            return code;
        }

        public static string GeneratePlayerDataSubscribeManagerScript(List<PlayerDataEditorData> datas, string category)
        {
            ClassGenerationData classData = new ClassGenerationData
            {
                m_ClassName = "PlayerDataSubscribeManager",
                m_Namespace = PlayerDataCodeGeneratorConstants.NAMESPACE_GAME,
                m_Usings = new string[] { "UniRx", PlayerDataCodeGeneratorConstants.NAMESPACE_FRAMEWORK },
                m_ClassType = ClassType.Partial
            };

            classData.m_MethodGenerationDatas.Add(PlayerDataMethodGenerator.GenerateSubscribeMethod(datas, category));

            string code = ClassGenerator.CreateClass(classData);
            return code;
        }

        public static string GeneratePlayerDataSaverScript(List<PlayerDataEditorData> datas, string category)
        {
            ClassGenerationData classData = new ClassGenerationData
            {
                m_ClassName = "PlayerDataSaver",
                m_Namespace = PlayerDataCodeGeneratorConstants.NAMESPACE_GAME,
                m_Usings = new string[] { "System", "System.Collections.Generic", "System.Linq", PlayerDataCodeGeneratorConstants.NAMESPACE_FRAMEWORK },
                m_ClassType = ClassType.Partial
            };

            classData.m_MethodGenerationDatas.AddRange(PlayerDataMethodGenerator.GenerateSaverMethods(datas, category));

            string code = ClassGenerator.CreateClass(classData);
            return code;
        }
    }
}
