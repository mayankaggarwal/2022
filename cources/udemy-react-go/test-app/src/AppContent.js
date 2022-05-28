import React, { Component } from 'react';

export default class AppContent extends Component {

  //state = {posts: []};

  constructor(props) {
    super(props);
    //this.listRef = React.createRef();

    this.handlePostChange = this.handlePostChange.bind(this);
  }

  handlePostChange(posts) {
    this.props.handlePostChange(posts);
  }

  anotherFunction = () => {
    console.log('This is another function');
  }
  fetchList = () => {
    fetch('https://jsonplaceholder.typicode.com/posts')
    .then((response) => response.json())
    .then(json => {
      //console.log(json);
      //let posts = document.getElementById('post-list');
    //  const posts = this.listRef.current;
    //   json.forEach(function(obj) {
    //     let li = document.createElement("li");
    //     li.appendChild(document.createTextNode(obj.title));
    //     posts.appendChild(li);
    //   });

    //this.setState({posts: json});
    this.handlePostChange(json);
    })
  }

  clickedItem = (x) => {
    console.log("clicked", x);
  }

  render() {
    return (
      <div>
        This is the content
        <br />
        <hr />
        <p onMouseEnter={this.anotherFunction}>this is some text</p>
        <button onClick={this.fetchList} className='btn btn-primary'>Fetch Data</button>

        <hr/>
        <p>{this.props.state.posts.length} items long</p>
        {/* <ul id="post-list" ref={this.listRef}></ul> */}
        <ul>
          {this.props.state.posts.map((c) => (
            <li key={c.id}>
              <a href="#!" onClick={() => this.clickedItem(c.id)}>
                {c.title}
              </a>
            </li>
          ))}
        </ul>
      </div>
    )
  }
}