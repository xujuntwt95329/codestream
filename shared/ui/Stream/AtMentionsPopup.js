import React, { Component } from "react";
import Headshot from "./Headshot";

// AtMentionsPopup expects an on/off switch determined by the on property
// on = show the popup, off = hide the popup
// a people list, which is the possible list of people to at-mention
// with the format:
// [id, nickname, full name, email, headshot, presence]
// and a prefix, which is used to filter/match against the list
export default class AtMentionsPopup extends Component {
	render() {
		if (!this.props.on) return null;

		const items = this.props.items;

		return (
			<div className="mentions-popup" ref={ref => (this._div = ref)}>
				<div className="body">
					<div className="instructions" onClick={event => this.handleClickInstructions()}>
						People matching <b>"@{this.props.prefix}"</b>
					</div>
					<ul className="compact at-mentions-list">
						{items.map(item => {
							let className = item.id == this.props.selected ? "hover" : "none";
							// the handleClickPerson event needs to fire onMouseDown
							// rather than onclick because there is a handleblur
							// event on the parent element that will un-render
							// this component
							return (
								<li
									className={className}
									key={item.id}
									onMouseEnter={event => this.handleMouseEnter(item.id)}
									onMouseDown={event => this.handleClickItem(item.id)}
								>
									{item.headshot && <Headshot size={18} person={item.headshot} />}
									<span className="username">{item.identifier}</span>{" "}
									{item.description && <span className="name">{item.description}</span>}
									{item.help && <span className="help">{item.help}</span>}
								</li>
							);
						})}
					</ul>
					<table>
						<tbody>
							<tr>
								<td>&uarr; or &darr; to navigate</td>
								<td>&crarr; to select</td>
								<td>esc to dismiss</td>
							</tr>
						</tbody>
					</table>
				</div>
			</div>
		);
	}

	handleMouseEnter(id) {
		return this.props.handleHoverAtMention(id);
	}

	handleClickItem(id) {
		return this.props.handleSelectAtMention(id);
	}

	handleClickInstructions() {
		return this.props.handleSelectAtMention();
	}

	handleClick = async event => {
		console.log("CLICK ON MENTION: " + event.target.innerHTML);
	};

	selectFirstAtMention() {
		// FIXME -- how to build this?
	}
}
