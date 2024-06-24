using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace BrowserLlamaCpp.Shared
{
    public enum TestedModels
    {
        
		[ModelUrl("https://huggingface.co/afrideva/TinyMistral-248M-SFT-v4-GGUF/resolve/main/tinymistral-248m-sft-v4.q8_0.gguf")]
        [Description("tinymistral-248m-sft-v4.q8_0 (265.26 MB)")]
        TinyMistral248MSftV4,

        [ModelUrl("https://huggingface.co/TheBloke/TinyLlama-1.1B-Chat-v1.0-GGUF/resolve/main/tinyllama-1.1b-chat-v1.0.Q4_K_M.gguf")]
        [Description("tinyllama-1.1b-chat-v1.0.Q4_K_M (669 MB)")]
        TinyLlama11BChatV10,

        [ModelUrl("https://huggingface.co/Qwen/Qwen1.5-1.8B-Chat-GGUF/resolve/main/qwen1_5-1_8b-chat-q3_k_m.gguf")]
        [Description("qwen1_5-1_8b-chat-q3_k_m (1.02 GB)")]
        Qwen1518BChat,

        [ModelUrl("https://huggingface.co/stabilityai/stablelm-2-zephyr-1_6b/resolve/main/stablelm-2-zephyr-1_6b-Q4_1.gguf")]
        [Description("stablelm-2-zephyr-1_6b-Q4_1 (1.07 GB)")]
        Stablelm2Zephyr16B,

        [ModelUrl("https://huggingface.co/TKDKid1000/phi-1_5-GGUF/resolve/main/phi-1_5-Q4_K_M.gguf")]
        [Description("phi-1_5-Q4_K_M (918 MB)")]
        Phi15,

        [ModelUrl("https://huggingface.co/TheBloke/phi-2-GGUF/resolve/main/phi-2.Q3_K_M.gguf")]
        [Description("phi-2.Q3_K_M (1.48 GB)")]
        Phi2
    }
    public enum ModelState
    {
        [Description("InActive")]
        InActive,
        [Description("Loading")]
        Loading,
        [Description("Running")]
        Running        
    }

    public class ModelUrlAttribute(string url) : Attribute
    {
        public string Url { get; } = url;
    }
}
