create table scorecards (
    id serial primary key,
    course_name varchar(100) not null,
    layout_name varchar(100) not null,
    date timestamp not null,
    hole_pars integer[] not null,
    unique (course_name, layout_name, date)
);

create table scorecard_players (
    scorecard_id integer not null references scorecards (id) on delete cascade,
    name varchar(100) not null,
    total integer not null,
    par integer not null,
    hole_scores integer[] not null,
    primary key (scorecard_id, name)
);
