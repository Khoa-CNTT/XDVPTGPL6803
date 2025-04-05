using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif
using UnityEngine;
using UnityEngine.UIElements;
using VNCreator.Editors.Graph;

namespace VNCreator
{
    public class SaveUtility
    {
#if UNITY_EDITOR
        public void SaveGraph(StoryObject _story, ExtendedGraphView _graph)
        {
            EditorUtility.SetDirty(_story);

            List<NodeData> nodes = new List<NodeData>();
            List<Link> links = new List<Link>();

            foreach (BaseNode _node in _graph.nodes.ToList().Cast<BaseNode>().ToList())
            {
                    nodes.Add(
                    new NodeData
                    {
                        guid = _node.nodeData.guid,
                        characterSpr = _node.nodeData.characterSpr,
                        characterName = _node.nodeData.characterName,
                        dialogueText = _node.nodeData.dialogueText,
                        backgroundSpr = _node.nodeData.backgroundSpr,
                        startNode = _node.nodeData.startNode,
                        endNode = _node.nodeData.endNode,
                        choices = _node.nodeData.choices,
                        choiceOptions = _node.nodeData.choiceOptions,
                        nodePosition = _node.GetPosition(),
                        soundEffect = _node.nodeData.soundEffect,
                        backgroundMusic = _node.nodeData.backgroundMusic
                    });
            }

            List<Edge> _edges = _graph.edges.ToList();
            for (int i = 0; i < _edges.Count; i++)
            {
                BaseNode _output = (BaseNode)_edges[i].output.node;
                BaseNode _input = (BaseNode)_edges[i].input.node;

                links.Add(new Link 
                { 
                    guid = _output.nodeData.guid,
                    targetGuid = _input.nodeData.guid,
                    portId = i
                });
            }

            _story.SetLists(nodes, links);

            //_story.nodes = nodes;
            //_story.links = links;
        }

        public void LoadGraph(StoryObject _story, ExtendedGraphView _graph)
        {
            foreach (NodeData _data in _story.nodes)
            {
                BaseNode _tempNode = _graph.CreateNode("", _data.nodePosition.position, _data.choices, _data.choiceOptions, _data.startNode, _data.endNode, _data);
                _graph.AddElement(_tempNode);
            }

            GenerateLinks(_story, _graph);
        }

        void GenerateLinks(StoryObject _story, ExtendedGraphView _graph)
        {
            List<BaseNode> _nodes = _graph.nodes.ToList().Cast<BaseNode>().ToList();

            for (int i = 0; i < _nodes.Count; i++)
            {
                int _outputIdx = 1;
                List<Link> _links = _story.links.Where(x => x.guid == _nodes[i].nodeData.guid).ToList();
                for (int j = 0; j < _links.Count; j++)
                {
                    string targetGuid = _links[j].targetGuid;
                    BaseNode _target = _nodes.First(x => x.nodeData.guid == targetGuid);
                    LinkNodes(_nodes[i].outputContainer[_links.Count > 1 ? _outputIdx : 0].Q<Port>(), (Port)_target.inputContainer[0], _graph);
                    _outputIdx += 2;
                }
            }
        }

        void LinkNodes(Port _output, Port _input, ExtendedGraphView _graph)
        {
            //Debug.Log(_output);

            Edge _temp = new Edge
            {
                output = _output,
                input = _input
            };

            _temp.input.Connect(_temp);
            _temp.output.Connect(_temp);
            _graph.Add(_temp);
        }
#endif
    }
}
