import React, { Component, Fragment } from 'react'
import {useParams} from 'react-router-dom';

class OneMovie extends Component {
  state = {movie:{}};
  componentDidMount() {
    const id = this.props.params.id
    console.log(id);
    this.setState({movie:{
      id: id,
      title: "Some Movie",
      runtime: 150,
    }});
  }
  render() {
    return (
      <Fragment>
        <h2>Movie: {this.state.movie.id}</h2>
        <table className='table table-compact table-striped'>
          <thead></thead>
          <tbody>
            <tr>
              <td><string>Title:</string></td>
              <td>{this.state.movie.title}</td>
            </tr>
            <tr>
              <td><string>Runtime:</string></td>
              <td>{this.state.movie.runtime} minutes</td>
            </tr>
          </tbody>
        </table>
      </Fragment>

    );
  }
}
export default withRouter(OneMovie);

function withRouter(Component) {
  function ComponentWithRouter(props) {
    let params = useParams()
    return <Component {...props} params={params} />
  }
  return ComponentWithRouter
}